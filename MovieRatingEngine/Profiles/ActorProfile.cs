﻿using AutoMapper;
using MovieRatingEngine.Dtos.Actor;
using MovieRatingEngine.Models;

namespace MovieRatingEngine.Profiles
{
    public class ActorProfile : Profile
    {
        public ActorProfile()
        {
            CreateMap<Actor, AddActorDto>();
            CreateMap<Actor, GetActorDto>();

            CreateMap<Actor, AddActorDto>().ReverseMap();
            CreateMap<Actor, GetActorDto>().ReverseMap();
        }
    }
}
