﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SensorData.Api.Settings;
using SensorData.SharedComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SensorData.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SensorDataServiceController : ControllerBase
    {
        private readonly ILogger<SensorDataServiceController> _logger;
        private readonly ITcpCommunicationClient<ApiCommandObject> _tcpClient;
        private readonly SensorDataServiceSettings _settings;

        public SensorDataServiceController(ILogger<SensorDataServiceController> logger,
            ITcpCommunicationClient<ApiCommandObject> tcpClient, IOptions<SensorDataServiceSettings> settings)
        {
            _logger = logger;
            _tcpClient = tcpClient;
            _settings = settings.Value;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok("Running");
        }

        [HttpPost]
        [Route("start")]
        public ActionResult Start()
        {
            _tcpClient.Connect(_settings);
            _tcpClient.Send(new ApiCommandObject(SensorDataOverTcpProtocol.ApiCommands.StartSensorDataService));
            return Ok();
        }

        [HttpPost]
        [Route("stop")]
        public ActionResult Stop()
        {
            _tcpClient.Send(new ApiCommandObject(SensorDataOverTcpProtocol.ApiCommands.StopSensorDataService));
            return Ok();
        }

        [HttpGet]
        [Route("sensorvalue/{id}")]
        public ActionResult FetchSensorValue(int id)
        {
            if (0 >= id || 255 < id)
            {
                return BadRequest();
            }

            ApiCommandObject apiCommandObject = _tcpClient.SendReceive(new ApiCommandObject(SensorDataOverTcpProtocol.ApiCommands.FetchSensorValue, (byte)id));
            return Ok(new { sensor_id = id, value = apiCommandObject.Data[0] });
        }

        //[HttpPost]
        //[Route("connect")]
        //public ActionResult Connect()
        //{
        //    _tcpClient.Connect(_settings);
        //    return Ok(new { Connected = _tcpClient.Connected });
        //}

        //[HttpPost]
        //[Route("disconnect")]
        //public ActionResult Disconnect()
        //{
        //    _tcpClient.Disconnect();
        //    return Ok(new { Connected = _tcpClient.Connected });
        //}
    }
}
