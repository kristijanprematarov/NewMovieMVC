﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewMovieMVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NewMovieMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<MovieViewModel> movies = new List<MovieViewModel>();

            using (var httpClient = new HttpClient())
            {
                using (var request = await httpClient.GetAsync("https://localhost:44358/api/movie/filmovi"))
                {
                    string apiResponse = await request.Content.ReadAsStringAsync();

                    movies = JsonConvert.DeserializeObject<IEnumerable<MovieViewModel>>(apiResponse);

                }
            }

            return View(movies);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            MovieViewModel movie = new MovieViewModel();

            using (var httpClient = new HttpClient())
            {
                using (var request = await httpClient.GetAsync("https://localhost:44358/api/movie/film/" + id.ToString()))
                {
                    string apiResponse = await request.Content.ReadAsStringAsync();

                    movie = JsonConvert.DeserializeObject<MovieViewModel>(apiResponse);

                }
            }

            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MovieViewModel movieModel)
        {
            var statusCode = string.Empty;

            using (var httpClient = new HttpClient())
            {
                HttpContent movie = new StringContent(movieModel.ToString(), Encoding.UTF8, "application/json");
                using (var request = await httpClient.PostAsync("https://localhost:44358/api/movie/film", movie))
                {
                    if (request.IsSuccessStatusCode)
                    {
                        statusCode += request.StatusCode.ToString();
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
