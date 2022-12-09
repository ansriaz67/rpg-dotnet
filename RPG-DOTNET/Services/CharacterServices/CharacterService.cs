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
using System.Security.Claims;
using Newtonsoft.Json;
using Azure;
using System.Web.Helpers;
using RPG_DOTNET.Repository;
using Org.BouncyCastle.Utilities.Collections;

namespace RPG_DOTNET.Services.CharacterServices
{
    public class CharacterService : ICharacterService
    {
        /*private static readonly List<Character> characters = new List<Character>
        {
            new Character(),
            new Character { Id = 1, Name = "Ans" }
        };*/

        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Character> _repository;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor, IRepository<Character> repository)
        {   
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponce<List<GetAllCharactersDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            ServiceResponce<List<GetAllCharactersDto>> serviceResponce = new ServiceResponce<List<GetAllCharactersDto>>();
            Character character = _mapper.Map<Character>(newCharacter);
            character.User = await _context.User.FirstOrDefaultAsync(u=>u.Id == GetUserId());
            _repository.Insert(character);
            /*await _context.Characters.AddAsync(character);*/
            await _context.SaveChangesAsync();
            serviceResponce.Data  = (_context.Characters.Select(c => _mapper.Map<GetAllCharactersDto>(c))).ToList();
            return serviceResponce;
        }

        public async Task<ServiceResponce<GetAllCharactersDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            ServiceResponce<GetAllCharactersDto> serviceResponse = new ServiceResponce<GetAllCharactersDto>();
            try { 
            Character character = await _context.Characters.Include(c => c.User).FirstOrDefaultAsync(a => a.Id == updateCharacter.Id);
            if(character != null)
            {
                    character.Name = updateCharacter.Name;
                    character.Class = updateCharacter.Class;
                    character.Defence = updateCharacter.Defence;
                    character.Intelligence = updateCharacter.Intelligence;
                    character.HitPoits = updateCharacter.HitPoits;
                    character.Strength = updateCharacter.Strength;
                    _context.Characters.Update(character);
                    await _context.SaveChangesAsync();
                    serviceResponse.Data = _mapper.Map<GetAllCharactersDto>(character);
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character Not Found";
                }
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
                Character character = await _context.Characters.FirstOrDefaultAsync(a => a.Id == id && a.User.Id == GetUserId());
                if (character != null)
                {
                _repository.Delete(character);
                await _context.SaveChangesAsync();
                serviceResponse.Data = (_context.Characters.Where(c => c.User.Id == GetUserId()).Select(c => _mapper.Map<GetAllCharactersDto>(c))).ToList();
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character not Found.";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponce<List<GetAllCharactersDto>>> GetAllCharacters(PagingParameterModel pagingparametermodel)
        {
            ServiceResponce<List<GetAllCharactersDto>> serviceResponce = new ServiceResponce<List<GetAllCharactersDto>>();
            /*List<Character> dbCharacters = await _context.Characters.Where(c => c.User.Id == GetUserId())
                .OrderBy(on => on.Name)
                .Skip((pagingparametermodel.PageNumber - 1) * pagingparametermodel.PageSize)
                .Take(pagingparametermodel.PageSize)
                .ToListAsync();*/
            var dbCharacters = _repository.GetAll();

            serviceResponce.Data = (dbCharacters.Select(c => _mapper.Map<GetAllCharactersDto>(c)).ToList());
            return serviceResponce;
        }

        public async Task<ServiceResponce<IEnumerable<GetAllCharactersDto>>> SearchCharacters(string searchText = "")
        {
            ServiceResponce<IEnumerable<GetAllCharactersDto>> serviceResponce = new ServiceResponce<IEnumerable<GetAllCharactersDto>>();
            try
            {
                if (searchText != null && searchText != "")
                {
                List<Character> characters = await _context.Characters.Where(s => s.Name.Contains(searchText)).ToListAsync();
                serviceResponce.Data = _mapper.Map<IEnumerable<GetAllCharactersDto>>(characters);
                }
                else
                {
                    serviceResponce.Success = false;
                    serviceResponce.Message = "User Not Found.";
                    return serviceResponce;
                }
            }
            catch (Exception ex)
            {
                serviceResponce.Success = false;
                serviceResponce.Message = ex.Message;
            }
            return serviceResponce;
        }

        public async Task<ServiceResponce<List<GetAllCharactersDto>>> SortingCharacters(string sortingText = "")
        {
            ServiceResponce<List<GetAllCharactersDto>> serviceResponce = new ServiceResponce<List<GetAllCharactersDto>>();
            try
            {
                if(sortingText == "name_desc")
                {
                    List<Character> characters = await _context.Characters.OrderByDescending(c => c.Name).ToListAsync();
                    serviceResponce.Data = _mapper.Map<List<GetAllCharactersDto>>(characters);
                }
                else
                {
                    List<Character> characters = await _context.Characters.OrderBy(c => c.Name).ToListAsync();
                    serviceResponce.Data = _mapper.Map<List<GetAllCharactersDto>>(characters);
                }
            }
            catch (Exception ex)
            {
                serviceResponce.Success = false;
                serviceResponce.Message = ex.Message;
            }
            return serviceResponce;
        }

        public async Task<ServiceResponce<GetAllCharactersDto>> GetCharacterById(int id)
        {
            ServiceResponce<GetAllCharactersDto> serviceResponce = new ServiceResponce<GetAllCharactersDto>();
            var dbCharacter = _repository.FindById(id);
            /*Character dbCharacter = await _context.Characters
                 .Include(c => c.Weapon).Include(c => c.CharacterSkills).ThenInclude(cs => cs.Skill)
                .FirstOrDefaultAsync(a => a.Id == id && a.User.Id == GetUserId());*/
            serviceResponce.Data = _mapper.Map<GetAllCharactersDto>(dbCharacter);
            return serviceResponce;
        }
    }
}