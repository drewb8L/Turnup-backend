using System.Text.Json;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Moq;
using NuGet.Packaging;
using NuGet.Protocol;
using Turnup.Controllers;
using Turnup.Entities;
using Turnup.Services;
using Turnup.Services.ScanService;

namespace Tests;


public class ScanControllerTests
{
  private Mock<IScanService> _scanService;
  private Fixture _fixture;
  private ScanController _controller;

  public ScanControllerTests()
  {
    _fixture = new Fixture();
    _scanService = new Mock<IScanService>();
  }

  [Fact]
  public async Task Get200WEstablishmentCodeIsValid()
  {
    var establishment= _fixture.Create<Establishment>();
    var sr = new ServiceResponse<List<Product>>()
    {
      Data = establishment.Products
    };
    _scanService.Setup(r => r.GetEstablishmentProducts(establishment.EstablishmentCode))
      .ReturnsAsync(sr);
    _controller = new ScanController(_scanService.Object);

    var result = await _controller.GetEstablishment(establishment.EstablishmentCode);

    var obj = result.Result as ObjectResult;

    Assert.Equal(200, obj.StatusCode);
   

  }

  [Fact]
  public async Task Get404WhenNoEstablishmentExists()
  {
    var establishment = _fixture.Create<Establishment>();
    var sr = new ServiceResponse<List<Product>>()
    {
      Data = establishment.Products
    };
    _scanService.Setup(r => r.GetEstablishmentProducts(establishment.EstablishmentCode))
      .ReturnsAsync(sr);
    _controller = new ScanController(_scanService.Object);

    var result = await _controller.GetEstablishment("Invalid Establishment Code");
    var obj = result.ToJson();
    Assert.Contains("404", result.ToJson());

}

  [Fact]
  public async Task GetProductsWhenEstablishmentCodeIsValid()
  {
    var establishment = _fixture.Create<Establishment>();

    var products = establishment.Products;

    var sr = new ServiceResponse<List<Product>>()
    {
      Data = new List<Product>()
    };
    sr.Data.AddRange(products);

    _scanService.Setup(r => r.GetEstablishmentProducts(establishment.EstablishmentCode))
      .ReturnsAsync(sr);

    _controller = new ScanController(_scanService.Object);
    var result = await _controller.GetEstablishment(establishment.EstablishmentCode);
    var obj = result.Result as ObjectResult;

    var jsonObj = JsonSerializer.Deserialize<List<Product>>(obj.Value.ToJson());
    
    Assert.Equal(3, jsonObj.Count);

  }

}
       