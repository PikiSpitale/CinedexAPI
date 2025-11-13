using AutoMapper;
using proyecto_prog4.Models.Genres.Dto;
using proyecto_prog4.Models.Genres;
using proyecto_prog4.Models.Movie;
using proyecto_prog4.Models.Movie.Dto;
using proyecto_prog4.Models.MovieGenres;
using proyecto_prog4.Models.User.Dto;
using proyecto_prog4.Models.Usuario;

namespace MovieExplorer.Config
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            //  Defaults
            CreateMap<bool?, bool>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<List<string>?, List<string>>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<DateTime?, DateTime>().ConvertUsing((src, dest) => src ?? dest);

            //  Movie
            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres));
            CreateMap<Movie, MoviesDTO>()
                .ForMember(dest => dest.PosterPath, opt => opt.MapFrom(src => src.PosterUrl))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres));
            CreateMap<CreateMovieDTO, Movie>()
                .ForMember(dest => dest.PosterUrl, opt => opt.MapFrom(src => src.PosterPath));

            CreateMap<UpdateMovieDTO, Movie>().ForAllMembers(opts =>
            {
                opts.Condition((_, _, srcMember) => srcMember != null);
            });

            //Genres
            CreateMap<Genre, GenreDTO>();
            CreateMap<MovieGenres, GenreDTO>().ConvertUsing(src => new GenreDTO
            {
                Id = src.GenreId,
                Name = src.Genre.Name ?? string.Empty
            });
            //  Usuario / Auth
            CreateMap<RegisterDTO, Usuario>();
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();

            CreateMap<Usuario, UsuarioWithRolesDTO>().ForMember(
                dest => dest.Roles,
                opt => opt.MapFrom(src => src.Roles.Select(r => r.Nombre).ToList())
            );
        }
    }
}



