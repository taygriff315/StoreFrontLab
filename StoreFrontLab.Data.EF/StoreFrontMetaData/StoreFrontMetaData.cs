using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFrontLab.Data.EF //.StoreFrontMetaData
{
    #region Manufacturer MetaData
    public class ManufacturerMetaData
    {
        
        [Required(ErrorMessage ="You must enter the name of the Manufacturer")]
        [StringLength(50, ErrorMessage ="Cannot contain more than 50 characters")]
        [Display(Name ="Manufacturer")]
        public string ManufacturerName { get; set; }
       

        
    }
        [MetadataType(typeof(ManufacturerMetaData))]
        public partial class Manufacturer
        {

        }
    #endregion

    #region Products MetaData
    public class ProductMetaData
    {
        //public short ProductID { get; set; }
        [Required(ErrorMessage ="*Required")]
        [Display(Name ="Product Type Id#")]
        public short ProductTypeID { get; set; }

        [Required(ErrorMessage = "*Required")]
        [Display(Name = "Manufacturer Id#")]
        public short ManufacturerID { get; set; }
       

        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        [Display(Name ="Units Sold")]
        [Range(0, int.MaxValue, ErrorMessage = "Value must be a valid number, 0 or larger")]
        public Nullable<int> UnitsSold { get; set; }

        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        [Display(Name ="In-Stock")]
        public Nullable<bool> InStock { get; set; }

        [Display(Name ="Image")]
        public string ProductImage { get; set; }

        [DisplayFormat(NullDisplayText ="[-N/A-]", DataFormatString ="{0:c}")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be a valid number. 0 or larger")]
        public Nullable<decimal> Price { get; set; }

        [Required(ErrorMessage ="Must enter a Product Name")]
        [Display(Name ="Product Name")]
        [StringLength(50, ErrorMessage ="Name cannont contain more than 50 characters")]
        public string ProductName { get; set; }
    }

    [MetadataType(typeof(ProductMetaData))]
    public partial class Product
    {

    }
    #endregion

    #region ProductTypeMetaData

    public class ProductTypeMetaData
    {
       // public short ProductTypeID { get; set; }
       [Display(Name ="Product Type")]
       [Required(ErrorMessage ="You must declare the product type")]
       [StringLength(50,ErrorMessage ="Product type cannot be more than 50 characters")]
        public string ProductTypeName { get; set; }
    }

    [MetadataType(typeof(ProductTypeMetaData))]
    public partial class ProductType
    {

    }

    #endregion






}
