﻿using System;
using Microsoft.AspNetCore.Mvc;
using RideWithMeWebApp.DataProvider.Models.Interfaces;
using RideWithMeWebApp.Models.Classes;
using RideWithMeWebApp.Models.Interfaces;

namespace RideWithMeWebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RidesController : Controller
    {
        private readonly IDataProvider _dataProvider;

        public RidesController()
        {
            _dataProvider = new DataProvider.Models.Classes.DataProvider();
        }

        [HttpGet("userId/{id}")]
        public WebResponse GetByUserId(long id)
        {
            var response = new WebResponse();
            response.Data["rideHistory"] = _dataProvider.GetRidesByUserId(id);
            return response;
        }

        [HttpGet("rideId/{id}")]
        public WebResponse GetByRideId(long id)
        {
            var response = new WebResponse();
            response.Data["rideHistory"] = _dataProvider.GetRidesById(id);
            return response;
        }

        [HttpGet("all/")]
        public WebResponse GetAllRides()
        {
            var response = new WebResponse();
            var data = _dataProvider.GetAllRides();
            if (data != null)
            {
                response.Message = "Success!";
                response.Data["rideHistory"] = _dataProvider.GetAllRides();
            }
            else
            {
                response.Message = "Error: Unable to GetAllRides.";
            }
            
            return response;
        }


        [HttpPost]
        [ProducesResponseType(200)]
        public OkResult Post([FromBody] Ride ride)
        {
            _dataProvider.AddNewRide(ride);

            return Ok();
        }

        [HttpGet("test")]
        public IRide Get()
        {
            var newRide = new Ride
            {
                Rider = new User
                {
                    FirstName = "Andy",
                    LastName = "Horn",
                    Email = "andyjhorn@gmail.com",
                    Id = 1,
                    UserType = 0
                },
                Driver = new User
                {
                    FirstName = "Krystal",
                    LastName = "Cruz",
                    Email = "kcruz191@gmail.com",
                    Id = 2,
                    UserType = 1
                },
                Vehicle = new Vehicle()
                {
                    Color = "White",
                    Driver = null,
                    Id = 1,
                    License = "771JJA",
                    Make = "Hyundai",
                    Model = "Elantra",
                    Year = 2012
                },
                Destination = "Portland, OR",
                Distance = 15.0,
                RequestTime = new DateTime(2018, 11, 30),
                StartTime = new DateTime(2018, 11, 30),
                EndTime = new DateTime(2018, 11, 30),
                PickupLocation = "Wilsonville, OR"
            };

            return newRide;
        }
    }
}