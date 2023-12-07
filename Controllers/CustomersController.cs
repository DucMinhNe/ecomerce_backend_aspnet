﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using e_commerce_backend.Models;
using Microsoft.AspNetCore.Hosting;

namespace e_commerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly EcommerceContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CustomersController(EcommerceContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
             _webHostEnvironment = webHostEnvironment;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
          if (_context.Customers == null)
          {
              return NotFound();
          }
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
          if (_context.Customers == null)
          {
              return NotFound();
          }
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, [FromForm] IFormFile ?customerImageFile, [FromForm] Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            // Lấy thông tin khách hàng cũ từ database
            var existingCustomer = await _context.Customers.FindAsync(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            // Kiểm tra xem có hình ảnh cũ không
            if (!string.IsNullOrEmpty(existingCustomer.CustomerImage))
            {
                // Xóa hình ảnh cũ
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingCustomer.CustomerImage);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            if (customerImageFile != null && customerImageFile.Length > 0)
            {
                // Lưu trữ hình ảnh mới
                var fileName = $"{DateTime.Now:yyyyMMddHHmmssfff}{Path.GetExtension(customerImageFile.FileName)}";
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "customer_images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await customerImageFile.CopyToAsync(stream);
                }

                // Cập nhật thông tin của khách hàng
                customer.CustomerImage = "customer_images/" + fileName;
            }

            // Cập nhật thông tin khách hàng
            _context.Entry(existingCustomer).CurrentValues.SetValues(customer);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer([FromForm] IFormFile ?customerImageFile, [FromForm] Customer customer)
        {

            if (customerImageFile != null && customerImageFile.Length > 0)
            {
                var fileName = $"{DateTime.Now:yyyyMMddHHmmssfff}{Path.GetExtension(customerImageFile.FileName)}";
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath,"customer_images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await customerImageFile.CopyToAsync(stream);
                }

                customer.CustomerImage = "customer_images/"+fileName;
            }

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
