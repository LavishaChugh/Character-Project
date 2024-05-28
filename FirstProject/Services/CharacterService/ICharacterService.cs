
namespace FirstProject.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters();

        Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id);

        Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter);

        Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id);

        Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter);

        Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterskillDto addCharacterskillDto);

    }
}
