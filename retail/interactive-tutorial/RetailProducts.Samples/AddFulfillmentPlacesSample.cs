﻿// Copyright 2022 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// [START retail_add_fulfillment_places]

using Google.Cloud.Retail.V2;
using Google.LongRunning;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Threading;

/// <summary>
/// The add fulfillment places sample class.
/// </summary>
public class AddFulfillmentPlacesSample
{
    /// <summary>
    /// Get the add fulfillment palces request.
    /// </summary>
    /// <param name="productName">The actual name of the retail product.</param>
    /// <returns>Add fulfillment places request.</returns>
    private static AddFulfillmentPlacesRequest GetAddFulfillmentRequest(string productName)
    {
        // The request timestamp
        DateTime requestTimeStamp = DateTime.Now.ToUniversalTime();

        // The outdated request timestamp
        // requestTimeStamp = DateTime.Now.ToUniversalTime().AddDays(-1);

        AddFulfillmentPlacesRequest addFulfillmentRequest = new AddFulfillmentPlacesRequest
        {
            Product = productName,
            Type = "pickup-in-store",
            AddTime = Timestamp.FromDateTime(requestTimeStamp),
            AllowMissing = true
        };

        string[] placeIds = { "store2", "store3", "store4" };

        addFulfillmentRequest.PlaceIds.AddRange(placeIds);

        Console.WriteLine("Add fulfillment places request:");
        Console.WriteLine($"Product Name: {addFulfillmentRequest.Product}");
        Console.WriteLine($"Type: {addFulfillmentRequest.Type}");
        Console.WriteLine($"Add Time: {addFulfillmentRequest.AddTime}");
        Console.WriteLine();

        return addFulfillmentRequest;
    }

    /// <summary>
    /// Call the Retail API to add fulfillment places.
    /// </summary>
    /// <param name="productName">The actual name of the retail product.</param>
    public static void AddFulfillment(string productName)
    {
        var addFulfillmentRequest = GetAddFulfillmentRequest(productName);
        GetProductServiceClient().AddFulfillmentPlaces(addFulfillmentRequest);

        // This is a long running operation and its result is not immediately present with get operations,
        // thus we simulate wait with sleep method.
        Console.WriteLine("\nAdd fulfillment places. Wait 2 minutes:");
        Thread.Sleep(120000);
    }

    /// <summary>
    /// Get product service client.
    /// </summary>
    private static ProductServiceClient GetProductServiceClient()
    {
        string Endpoint = "retail.googleapis.com";

        var productServiceClientBuilder = new ProductServiceClientBuilder
        {
            Endpoint = Endpoint
        };

        var productServiceClient = productServiceClientBuilder.Build();
        return productServiceClient;
    }
}
// [END retail_add_fulfillment_places]

/// <summary>
/// The add fulfillment places tutorial class.
/// </summary>
public static class AddFulfillmentPlacesTutorial
{
    [Runner.Attributes.Example]
    public static void PerformAddFulfillment()
    {
        string projectId = Environment.GetEnvironmentVariable("GOOGLE_PROJECT_ID");

        // Create product.
        Product createdProduct = CreateProductSample.CreateRetailProductWithFulfillment(projectId);

        // Add fulfillment info to created product.
        AddFulfillmentPlacesSample.AddFulfillment(createdProduct.Name);

        // Get product.
        GetProductSample.GetRetailProduct(createdProduct.Name);

        // Delete product.
        DeleteProductSample.DeleteRetailProduct(createdProduct.Name);
    }
}