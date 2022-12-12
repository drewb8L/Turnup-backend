using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;
using AutoFixture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NuGet.Protocol;
using Turnup.Controllers;
using Turnup.Entities;
using Turnup.Services;
using Turnup.Services.ProductService;

namespace Tests;

public class ProductControllerTests
{
    private Mock<IProductService> _productService;
    private Fixture _fixture;
    private ProductController _controller;

    public ProductControllerTests()
    {
        _fixture = new Fixture();
        _productService = new Mock<IProductService>();

    }
    
    

    [Fact]
    [Authorize]
    public async Task GetProductsThisEstablishmentOwns()
    {
        var establishment= _fixture.Create<Establishment>();
        var claimsPrincipal = ClaimsPrincipal(establishment, out var claims, out var identity);


        var sr = new ServiceResponse<List<Product>>()
        {
            Data = establishment.Products
        };
        
        _productService.Setup(p => p.GetProductsAsync(establishment.Id.ToString()))
            .ReturnsAsync(sr);

        _controller = new ProductController(_productService.Object);
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = claimsPrincipal
            }
        };
        
        var result = await _controller.GetProducts();

        var obj = result.Result as ObjectResult;
        var jsonObj = JsonSerializer.Deserialize<ServiceResponse<List<Product>>>(obj.Value.ToJson());
    
        Assert.True(jsonObj.Success);
        Assert.Equal(3, jsonObj.Data.Count);
    }

    


    [Fact]
    [Authorize]
    public async Task ReturnThisEstablishmentsWithDetails()
    {
        
        var establishment = _fixture.Create<Establishment>();
        var sr = new ServiceResponse<Establishment>()
        {
            Data = establishment
        };
        _productService.Setup(e => e.GetEstablishmentDetails(establishment.Id.ToString()))
            .ReturnsAsync(sr);
        _controller = new ProductController(_productService.Object);

        var result = await _controller.GetEstablishment(establishment.Id.ToString());
        var obj = result.Result as OkObjectResult;
        var jsonObj = JsonSerializer.Deserialize<Establishment>(obj.Value.ToJson());

        Assert.NotNull(jsonObj);
        Assert.Contains("EstablishmentCode", jsonObj.EstablishmentCode);
    }

    [Fact]
    [Authorize]
    public async Task CreateNewProduct()
    {
        var establishment = _fixture.Create<Establishment>();
        var claimsPrincipal = ClaimsPrincipal(establishment, out var claims, out var identity);
        var product = _fixture.Create<Product>();
        var sr = new ServiceResponse<Product>()
        {
            Data = product
        };

        _productService.Setup(p => p.CreateNewProduct(product.Title, product.Description, product.ImageUrl,
                product.Price, establishment.Id.ToString()))
            .ReturnsAsync(sr);
        _controller = new ProductController(_productService.Object);
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = claimsPrincipal
            }
        };

        var result = await _controller
            .CreateNewProduct(product.Title, product.Description, product.ImageUrl, product.Price);
        
        var obj = result.Result as OkObjectResult;
        var jsonObj = JsonSerializer.Deserialize<ServiceResponse<Product>>(obj.Value.ToJson());
        
        Assert.NotNull(jsonObj.Data);
        Assert.Matches(new Regex("[A-Za-z0-9]+-.*-.*-.*-", RegexOptions.IgnoreCase), jsonObj.Data.EstablishmentId);


    }
    
    
    
    
    
    private static ClaimsPrincipal ClaimsPrincipal(Establishment establishment, out List<Claim> claims,
        out ClaimsIdentity identity)
    {
        claims = new List<Claim>()
        {
            new Claim("Id", establishment.Id.ToString()),
            new Claim(ClaimTypes.Role, Roles.establishment.ToString()),
        };
        identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        return claimsPrincipal;
    }
    
}