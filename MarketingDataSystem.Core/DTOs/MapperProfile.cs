using AutoMapper;
using MarketingDataSystem.Core.Entities;

namespace MarketingDataSystem.Core.DTOs
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UsuarioMarketing, UsuarioMarketingDto>().ReverseMap();
            CreateMap<Reporte, ReporteDto>().ReverseMap();
            CreateMap<DescargaReporte, DescargaReporteDto>().ReverseMap();
            CreateMap<FuenteDeDatos, FuenteDeDatosDto>().ReverseMap();
            CreateMap<IngestionLog, IngestionLogDto>().ReverseMap();
            CreateMap<DatoCrudo, DatoCrudoDto>().ReverseMap();
            CreateMap<DatoNormalizado, DatoNormalizadoDto>().ReverseMap();
            CreateMap<Metadata, MetadataDto>().ReverseMap();
            CreateMap<Stock, StockDto>().ReverseMap();
            CreateMap<Producto, ProductoDto>().ReverseMap();
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<Venta, VentaDto>().ReverseMap();
        }
    }
} 