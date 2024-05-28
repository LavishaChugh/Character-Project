using FirstProject.Dtos.Weapon;

namespace FirstProject.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto addWeapon);
    }
}
