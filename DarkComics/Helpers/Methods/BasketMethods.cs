using DarkComics.DAL;
using DarkComics.Helpers.Enums;
using DarkComics.Models.Entity;
using DarkComics.ViewModels;
using DarkComics.ViewModels.Basket;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DarkComics.Helpers.Methods
{
    public static class BasketMethod
    {
       
        public static List<BasketProduct> AddBasket(Product basketItem, string cookie)
        {
            List<BasketProduct> temporaryList = new List<BasketProduct>();

            if (string.IsNullOrEmpty(cookie))
            {
                BasketProduct temporaryProduct = new BasketProduct
                {
                    Id = basketItem.Id,
                    Count = 1
                };
                temporaryList.Add(temporaryProduct);

            }
            else
            {
                temporaryList = JsonSerializer.Deserialize<List<BasketProduct>>(cookie);
                var temporaryProduct = temporaryList.FirstOrDefault(tp => tp.Id == basketItem.Id);

               
                    if (temporaryProduct == null && basketItem.Quantity > 0)
                    {                    
                        temporaryProduct = new BasketProduct
                        {
                            Id = basketItem.Id,
                            Count = 1
                        };
                        temporaryList.Add(temporaryProduct);
                                        
                    }
                    else if(temporaryProduct != null && (basketItem.Quantity - temporaryProduct.Count) > 0)
                    {
                        temporaryProduct.Count++;
                    }
                
               
            }
        
            //Response.Cookies.Append("basket", JsonSerializer.Serialize(temporaryList));

            return temporaryList;
        }
        public static BasketViewModel ShowBasket(List<Product> products,string cookie)
        {
            BasketViewModel basketVM = new BasketViewModel
            {
                TotalCount = 0,
                TotalPrice = 0,
                ProductDetails = new List<BasketItemViewModel>()
            };

            if (cookie != null)
            {
                var tempList = JsonSerializer.Deserialize<List<BasketProduct>>(cookie);

                if (tempList.FirstOrDefault() != null)
                {
                    foreach (var temporaryProduct in tempList)
                    {
                        if (temporaryProduct != null)
                        {
                            var basketItem = products.FirstOrDefault(p => p.Id == temporaryProduct.Id && p.IsActive == true);

                            if (basketItem != null)
                            {
                                BasketItemViewModel basketItemViewModel = new BasketItemViewModel
                                {

                                    Product = basketItem,
                                    Count = temporaryProduct.Count
                                };
                                basketVM.ProductDetails.Add(basketItemViewModel);
                                basketVM.TotalCount++;
                                basketVM.TotalPrice += Convert.ToDecimal(basketItem.Price * basketItemViewModel.Count);
                            }
                        }
                    }
                }
            }
            return basketVM;
        }
    }
}
