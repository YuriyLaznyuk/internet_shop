using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using shop_brand.Dto;
using shop_brand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop_brand.Services
{
    public class BrandService
    {
        private AppDbContext _db;
        public BrandService(AppDbContext db)
        {
            _db = db;
        }
        private DbSet<Brand> _brand => _db.Brand;

        public static BrandDto ToBrandDto(Brand brand)
        {
            if(brand==null)
            {
                // throw new ArgumentException(nameof(brand));
                return null;
            }
            return new BrandDto
            {
                Id = brand.Id,
                Name = brand.Name,
                Value = brand.Value

            };
        }

        public List<BrandDto>GetAllBrand()
        {
            // return  _brand.ToList();
            // return ToBrandDto(_brand.ToList());
            List<BrandDto> list = new List<BrandDto>();
            var brandList = _brand.ToList();
            for (int i=0; i < brandList.Count; i++)
            {
                list.Add(ToBrandDto(brandList[i]));
            }
            return list;


        }

        public BrandDto  GetBrandById(int id)
        {
            var brand = _brand.SingleOrDefault((Brand brand) => brand.Id == id);
                if (brand==null)
            {
                return null;
            }
           

            else
            {
                var brand1 = ToBrandDto(brand);
                return brand1;
            }




        }

        public (bool result, Exception exception)DeleteBrandById(int id)
        {
            Brand brand = _brand.SingleOrDefault((Brand brand) => brand.Id == id);
            if (brand==null)
            {
                return (false, new ArgumentException($"User with id:{id} not found"));
            }
            EntityEntry<Brand> result = _brand.Remove(brand);

            try
            {
                _db.SaveChanges();
            }

            catch(Exception e)
            {
                return (false, new DbUpdateException($"Cannot save changes:{e.Message}"));

            }
            return (result.State == EntityState.Deleted, null);
        }
        public BrandDto AddBrand(string name, int value)
        {
            Brand brand = ToEntity(name, value);
            _brand.Add(brand);

            try
            {
                _db.SaveChanges();
            }
            catch
            {
                return null;
            }
            return ToBrandDto(brand);
        }

        public Brand ToEntity(string name, int value)
        {
            return new Brand {Name=name, Value=value };
        }

        public (BrandDto brandDto, Exception exception)UpdateBrand(Brand _brand)
        {
            Brand brand = this._brand.SingleOrDefault((Brand brand) => brand.Id == _brand.Id);
            if(brand==null)
            {
                return (null, new ArgumentException($"brand with id:{_brand.Id}not found"));

            }
            if(_brand.Id!=0)
            {
                brand.Value = _brand.Value;
                brand.Name = _brand.Name;
            }

            try
            { 
                _db.SaveChanges();
            }
            catch(Exception e)
            {
                return (null, new DbUpdateException($"Cannot save changes: {e.Message}"));
            }
            return (ToBrandDto(_brand), null);
        }
    }
}
