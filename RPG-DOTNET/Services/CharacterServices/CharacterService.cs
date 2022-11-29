using RPG_DOTNET.Services;
using RPG_DOTNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RPG_DOTNET.Dtos.CharacterDto;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPG_DOTNET.Data;

namespace RPG_DOTNET.Services.CharacterServices
{
    public class CharacterService : ICharacterService
    {
        private static readonly List<Character> characters = new List<Character>
        {
            new Character(),
            new Character { Id = 1, Name = "Ans" }
        };

        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public CharacterService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<ServiceResponce<List<GetAllCharactersDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            ServiceResponce<List<GetAllCharactersDto>> serviceResponce = new ServiceResponce<List<GetAllCharactersDto>>();
            Character character = _mapper.Map<Character>(newCharacter);
            await _context.Characters.AddAsync(character);
            await _context.SaveChangesAsync();
            serviceResponce.Data  = (_context.Characters.Select(c => _mapper.Map<GetAllCharactersDto>(c))).ToList();
            return serviceResponce;
        }

        public async Task<ServiceResponce<GetAllCharactersDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            ServiceResponce<GetAllCharactersDto> serviceResponse = new ServiceResponce<GetAllCharactersDto>();
            try { 
            Character character = await _context.Characters.FirstOrDefaultAsync(a => a.Id == updateCharacter.Id);   
            character.Name = updateCharacter.Name;
            character.Class = updateCharacter.Class;
            character.Defence = updateCharacter.Defence;
            character.Intelligence = updateCharacter.Intelligence;
            character.HitPoits = updateCharacter.HitPoits;
            character.Strength= updateCharacter.Strength;
            _context.Characters.Update(character);
            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<GetAllCharactersDto>(character);
            }
            catch (Exception ex) {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponce<List<GetAllCharactersDto>>> DeleteCharacter(int id)
        {
            ServiceResponce<List<GetAllCharactersDto>> serviceResponse = new ServiceResponce<List<GetAllCharactersDto>>();
            try
            {
                Character character = await _context.Characters.FirstAsync(a => a.Id == id);
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                serviceResponse.Data = (_context.Characters.Select(c => _mapper.Map<GetAllCharactersDto>(c))).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        async Task<ServiceResponce<List<GetAllCharactersDto>>> ICharacterService.GetAllCharacters()
        {
            ServiceResponce<List<GetAllCharactersDto>> serviceResponce = new ServiceResponce<List<GetAllCharactersDto>>();
            List<Character> dbCharacters = await _context.Characters.ToListAsync();
            serviceResponce.Data = (dbCharacters.Select(c => _mapper.Map<GetAllCharactersDto>(c))).ToList();
            return serviceResponce;
        }

        async Task<ServiceResponce<GetAllCharactersDto>> ICharacterService.GetCharacterById(int id)
        {
            ServiceResponce<GetAllCharactersDto> serviceResponce = new ServiceResponce<GetAllCharactersDto>();
            Character dbCharacter = await _context.Characters.FirstOrDefaultAsync(a => a.Id == id);
            serviceResponce.Data = _mapper.Map<GetAllCharactersDto>(dbCharacter);
            return serviceResponce;
        }
    }
}