using CRUDTut.Models;
using CRUDTut.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRUDTut.Pages.Admin.Products
{
    public class CreateModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly ApplicationDbContext context;

        [BindProperty]
        public ProductDto ProductDto { get; set; } = new ProductDto();

        public CreateModel(IWebHostEnvironment environment, ApplicationDbContext context)
        {
            this.environment = environment;
            this.context = context;
        }
        public void OnGet()
        {
        }

        public string errorMessage = "";
        public string successMessage = "";
        public void OnPost()
        {
            if (ProductDto.ImageFile == null) 
            {
                ModelState.AddModelError("ProductDto.ImageFile", "The image file is required");
            }

            if (!ModelState.IsValid)
            {
                errorMessage = "Please provide all the required fields";
                return;
            }


            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(ProductDto.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                ProductDto.ImageFile.CopyTo(stream);
            }

            Product product = new()
            {
                Name = ProductDto.Name,
                Brand = ProductDto.Brand,
                Category = ProductDto.Category,
                Price = ProductDto.Price,
                Description = ProductDto.Description ?? "",
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now,
            };

            context.Products.Add(product);
            context.SaveChanges();

            ProductDto.Name = "";
            ProductDto.Brand = "";
            ProductDto.Price = 0;
            ProductDto.Category = "";
            ProductDto.ImageFile = null;
            ProductDto.Description = "";

            ModelState.Clear();

            successMessage = "Product created successfully";

            Response.Redirect("/Admin/Products/Index");
        }
    }
}
