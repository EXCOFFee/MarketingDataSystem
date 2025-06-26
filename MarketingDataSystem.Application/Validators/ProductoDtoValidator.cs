using FluentValidation;
using MarketingDataSystem.Core.DTOs;
using System.Text.RegularExpressions;

namespace MarketingDataSystem.Application.Validators
{
    /// <summary>
    /// Validador para ProductoDto usando FluentValidation.
    /// Cumple con el principio de responsabilidad única (S de SOLID).
    /// </summary>
    public class ProductoDtoValidator : AbstractValidator<ProductoDto>
    {
        public ProductoDtoValidator()
        {
            // Validación de Nombre
            RuleFor(x => x.Nombre)
                .NotEmpty()
                .WithMessage("El nombre del producto es obligatorio")
                .MinimumLength(3)
                .WithMessage("El nombre debe tener al menos 3 caracteres")
                .MaximumLength(200)
                .WithMessage("El nombre no puede exceder 200 caracteres")
                .Must(BeValidProductName)
                .WithMessage("El nombre contiene caracteres no válidos");

            // Validación de Precio
            RuleFor(x => x.Precio)
                .GreaterThan(0)
                .WithMessage("El precio debe ser mayor a 0")
                .LessThan(999999.99f)
                .WithMessage("El precio no puede exceder $999,999.99");

            // Validación de Categoría
            RuleFor(x => x.Categoria)
                .NotEmpty()
                .WithMessage("La categoría es obligatoria")
                .MaximumLength(100)
                .WithMessage("La categoría no puede exceder 100 caracteres")
                .Must(BeValidCategory)
                .WithMessage("La categoría contiene caracteres no válidos");

            // Validación de Proveedor
            RuleFor(x => x.Proveedor)
                .NotEmpty()
                .WithMessage("El proveedor es obligatorio")
                .MaximumLength(150)
                .WithMessage("El proveedor no puede exceder 150 caracteres");

            // Validación de ID para actualizaciones
            When(x => x.IdProducto > 0, () =>
            {
                RuleFor(x => x.IdProducto)
                    .GreaterThan(0)
                    .WithMessage("El ID del producto debe ser mayor a 0");
            });

            // Validaciones de seguridad
            RuleFor(x => x)
                .Must(NoContenerScripts)
                .WithMessage("Los datos del producto no pueden contener scripts maliciosos");
        }

        /// <summary>
        /// Valida que el nombre del producto sea apropiado
        /// </summary>
        private bool BeValidProductName(string? nombre)
        {
            if (string.IsNullOrEmpty(nombre)) return false;
            
            // Permite letras, números, espacios y algunos caracteres especiales comunes
            return Regex.IsMatch(nombre, @"^[a-zA-ZÀ-ÿ\u00f1\u00d1\d\s\-\(\)\.\,\&\+]+$");
        }

        /// <summary>
        /// Valida que la categoría sea apropiada
        /// </summary>
        private bool BeValidCategory(string? categoria)
        {
            if (string.IsNullOrEmpty(categoria)) return false;
            
            // Permite letras, números, espacios y algunos caracteres especiales
            return Regex.IsMatch(categoria, @"^[a-zA-ZÀ-ÿ\u00f1\u00d1\d\s\-\&]+$");
        }

        /// <summary>
        /// Valida que el producto completo no contenga scripts maliciosos
        /// </summary>
        private bool NoContenerScripts(ProductoDto producto)
        {
            var scriptPatterns = new[]
            {
                @"<script[^>]*>.*?</script>",
                @"javascript:",
                @"vbscript:",
                @"on\w+\s*=",
                @"<iframe[^>]*>",
                @"<object[^>]*>",
                @"<embed[^>]*>"
            };

            var fields = new[] { producto.Nombre, producto.Categoria, producto.Proveedor };

            foreach (var field in fields)
            {
                if (string.IsNullOrEmpty(field)) continue;

                foreach (var pattern in scriptPatterns)
                {
                    if (Regex.IsMatch(field, pattern, RegexOptions.IgnoreCase))
                        return false;
                }
            }

            return true;
        }
    }
} 