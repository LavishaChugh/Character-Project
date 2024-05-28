
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FirstProject.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {

        private static List<Character> characters = new List<Character>()
        {
            new Character(),
            new Character {Id= 1, Name = "Sam"}
        };

        private readonly IMapper _mapper;

        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);



        //ADD new character(POST)
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var ServiceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter);
            character.User = await _context.Users.FirstOrDefaultAsync(x => x.Id == GetUserId());

            
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            ServiceResponse.Data = 
                await _context.Characters.Where(c => c.User!.Id == GetUserId()).Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return ServiceResponse;
        }


        //DELETE character
        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {

                var character =
                    await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());

                if (character is null)
                    throw new Exception("NOT FOUND");

                _context.Characters.Remove(character);

                await _context.SaveChangesAsync();

                serviceResponse.Data = 
                    await _context.Characters.Where(c => c.User!.Id == GetUserId()).Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }


        //list of ALL characters
        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters.Include(c => c.Weapon).Include(c => c.Skills).Where(c => c.User!.Id == GetUserId()).ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }


        //GET character by ID
        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await _context.Characters.Include(c => c.Weapon).Include(c => c.Skills).FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {

            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {

                var character = 
                    await _context.Characters.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);

                if (character is null || character.User!.Id != GetUserId())
                    throw new Exception("NOT FOUND");

                _mapper.Map(updatedCharacter, character);

                character.Name = updatedCharacter.Name;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Strength = updatedCharacter.Strength;
                character.Defense = updatedCharacter.Defense;
                character.Class = updatedCharacter.Class;
                character.Intelligence = updatedCharacter.Intelligence;

                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            } catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterskillDto newCharacterskill)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == newCharacterskill.CharacterId && c.User!.Id == GetUserId());

                if (character is null)
                {
                    response.Success = false;
                    response.Message = "Not found";
                    return response;
                }

                var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterskill.SkillId);

                if (skill is null)
                {
                    response.Success = false;
                    response.Message = "Not found";
                    return response;
                }


                character.Skills!.Add(skill);
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetCharacterDto>(character);

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;

            }

            return response;
        }
    }
}
