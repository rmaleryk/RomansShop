using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RomansShop.Core.Validation;
using RomansShop.Domain.Entities;
using RomansShop.Domain.Extensibility.Repositories;
using RomansShop.Services.Extensibility;
using RomansShop.WebApi.ClientModels.Order;
using RomansShop.WebApi.Filters;

namespace RomansShop.WebApi.Controllers
{
    [Route("api/orders")]
    [TypeFilter(typeof(ValidateModelAttribute))]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IOrderRepository orderRepository, IMapper mapper)
        {
            _orderService = orderService;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        // api/orders
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Order> orders = _orderRepository.GetAll();
            IEnumerable<OrderResponseModel> orderResponse = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderResponseModel>>(orders);

            return Ok(orderResponse);
        }

        // api/orders/status/
        [HttpGet("status/{status}")]
        public IActionResult GetByStatus(OrderStatus status)
        {
            IEnumerable<Order> orders = _orderRepository.GetByStatus(status);
            IEnumerable<OrderResponseModel> orderResponse = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderResponseModel>>(orders);

            return Ok(orderResponse);
        }

        // api/users/orders
        [HttpGet("/api/users/{userId}/orders")]
        public IActionResult GetByUserId(Guid userId)
        {
            IEnumerable<Order> orders = _orderRepository.GetByUserId(userId);
            IEnumerable<OrderResponseModel> orderResponse = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderResponseModel>>(orders);

            return Ok(orderResponse);
        }

        // api/orders/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            ValidationResponse<Order> validationResponse = _orderService.GetById(id);

            if (validationResponse.Status == ValidationStatus.NotFound)
            {
                return NotFound(validationResponse.Message);
            }

            OrderResponseModel orderResponse = _mapper.Map<Order, OrderResponseModel>(validationResponse.ResponseData);

            return Ok(orderResponse);
        }

        // api/orders
        [HttpPost]
        public IActionResult Post([FromBody]OrderRequestModel orderRequest)
        {
            Order order = _mapper.Map<OrderRequestModel, Order>(orderRequest);

            ValidationResponse<Order> validationResponse = _orderService.Add(order);

            if (validationResponse.Status == ValidationStatus.Failed)
            {
                return BadRequest(validationResponse.Message);
            }

            OrderResponseModel orderResponse = _mapper.Map<Order, OrderResponseModel>(validationResponse.ResponseData);

            return CreatedAtAction("Get", new { id = orderResponse.Id }, orderResponse);
        }

        // api/orders/{id}
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]OrderRequestModel orderRequest)
        {
            Order order = _mapper.Map<OrderRequestModel, Order>(orderRequest);
            order.Id = id;

            ValidationResponse<Order> validationResponse = _orderService.Update(order);

            if (validationResponse.Status == ValidationStatus.NotFound)
            {
                return NotFound(validationResponse.Message);
            }

            if (validationResponse.Status == ValidationStatus.Failed)
            {
                return BadRequest(validationResponse.Message);
            }

            OrderResponseModel orderResponse = _mapper.Map<Order, OrderResponseModel>(validationResponse.ResponseData);

            return Ok(orderResponse);
        }

        // api/orders/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            ValidationResponse<Order> validationResponse = _orderService.Delete(id);

            if (validationResponse.Status == ValidationStatus.NotFound)
            {
                return NotFound(validationResponse.Message);
            }

            if(validationResponse.Status == ValidationStatus.Failed)
            {
                return BadRequest(validationResponse.Message);
            }

            return Ok(validationResponse.Message);
        }
    }
}