using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using ValantDemoApi.Model;

namespace ValantDemoApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MazeController : ControllerBase
    {
        private readonly ILogger<MazeController> _logger;
        private IWebHostEnvironment _environment;
        public static List<List<MazeCell>> _mazeGrid = new List<List<MazeCell>>();
        public static MazeCell _currentMaze = new MazeCell();
        private int currentMazeRow;
        private int currentMazeCol;

    public MazeController(ILogger<MazeController> logger, IWebHostEnvironment enviroment)
        {
            _logger = logger;
            _environment = enviroment;
            /*_mazeGrid = new List<List<MazeCell>>();
             _currentMaze = new MazeCell() ;*/
        }

        /*[HttpGet]
        public IEnumerable<string> GetNextAvailableMoves()
        {
          return new List<string> {"Up", "Down", "Left", "Right"};
        

        }*/

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("MazeResource");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var mazePath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                      file.CopyTo(stream);
                    }

                    return Ok(new { mazePath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch(Exception ex)
            {
                    return StatusCode(500, "error reading Maze");
            }
           
        }

       [HttpGet]
        public JsonResult getFileNames()
            {
                string[] filepaths = Directory.GetFiles(Path.Combine(this._environment.ContentRootPath, "MazeResource/"));
                List<FileModel> list = new List<FileModel>();
                foreach(string filepath in filepaths)
                {
                   var s = Path.GetFileName(filepath);
                  list.Add(new FileModel { FileName = s.Substring(0, s.Length - 4)});
                }
                return new JsonResult(list);
            }

          [HttpGet("{mazename}")]
          public JsonResult loadMaze(string mazename)
          {
            _mazeGrid.Clear();
            int lineCounter = 0;
            string line;
            string path = Path.Combine(this._environment.ContentRootPath, "MazeResource/") + mazename + ".txt";
            StreamReader file = new StreamReader(path);
            List<string> testTextList = new List<string>();
            
            

            // read each line
            while ((line = file.ReadLine()) != null)
                  {
              List<MazeCell> mazeRow = new List<MazeCell>();
              for (int j =0; j < line.Length; j++)
                    {
                      MazeCell mc = new MazeCell();
                      mc.Symbol = line[j];
                      mc.Row = lineCounter;
                      mc.Col = j;
                      mazeRow.Add(mc);
                    
                    }
              _mazeGrid.Add(mazeRow);
              lineCounter++;
            }

            for(int i = 0; i < _mazeGrid.Count; i++)
            {
                for(int j = 0; j < _mazeGrid[i].Count; j++)
                {
                    if(_mazeGrid[i][j].Symbol == 'S')
                    {
            
                        _currentMaze = _mazeGrid[i][j];
                        _mazeGrid[i][j].currentLocation = true;
                        currentMazeRow = i;
                         currentMazeCol = j;
                    }
                    if(j + 1 < _mazeGrid[i].Count && _mazeGrid[i][j + 1].Symbol == 'O' && _mazeGrid[i][j].Symbol != 'X')
                    {
                        _mazeGrid[i][j].East = true;
                    }
                    if (i + 1 < _mazeGrid.Count && _mazeGrid[i + 1][j].Symbol == 'O' && _mazeGrid[i][j].Symbol != 'X')
                    {
                      _mazeGrid[i][j].South = true;
                    }

                    if (j - 1 >= 0 && _mazeGrid[i][j - 1].Symbol == 'O' && _mazeGrid[i][j].Symbol != 'X')
                    {
                      _mazeGrid[i][j].West = true;
                    }

                    if (i - 1 >= 0 && _mazeGrid[i - 1][j].Symbol == 'O' && _mazeGrid[i][j].Symbol != 'X')
                    {
                      _mazeGrid[i][j].North = true;
                    }
            }
          }     
            return new JsonResult(_mazeGrid);
          }

          [HttpPost]
          public JsonResult GoEast(MazeCell mc)
          {
            if(mc.Col + 1 >= _mazeGrid[mc.Row].Count)
              {
                return new JsonResult(false);
              }
            if (_mazeGrid[mc.Row][mc.Col + 1].Symbol == 'O' || _mazeGrid[mc.Row][mc.Col + 1].Symbol == 'S' || _mazeGrid[mc.Row][mc.Col + 1].Symbol == 'E')
            {
              _mazeGrid[_currentMaze.Row][_currentMaze.Col].currentLocation = false;
              _mazeGrid[mc.Row][mc.Col + 1].currentLocation = true;
              _currentMaze = _mazeGrid[mc.Row][mc.Col + 1];
              return new JsonResult(_mazeGrid);
            }
            return new JsonResult(false);
          }
          [HttpPost]
          public JsonResult GoWest(MazeCell mc)
          {
            if (mc.Col - 1 < 0)
            {
              return new JsonResult(false);
            }
            if (_mazeGrid[mc.Row][mc.Col - 1].Symbol == 'O' || _mazeGrid[mc.Row][mc.Col - 1].Symbol == 'S' || _mazeGrid[mc.Row][mc.Col - 1].Symbol == 'E')
            {
              _mazeGrid[_currentMaze.Row][_currentMaze.Col].currentLocation = false;
              _mazeGrid[mc.Row][mc.Col - 1].currentLocation = true;
              _currentMaze = _mazeGrid[mc.Row][mc.Col - 1];
              return new JsonResult(_mazeGrid);
            }
            return new JsonResult(false);
          }

          [HttpPost]
          public JsonResult GoNorth(MazeCell mc)
          {
            if (mc.Row - 1 < 0)
            {
              return new JsonResult(false);
            }
            if (_mazeGrid[mc.Row - 1][mc.Col].Symbol == 'O' || _mazeGrid[mc.Row - 1][mc.Col].Symbol == 'S' || _mazeGrid[mc.Row - 1][mc.Col].Symbol == 'E')
              {
                _mazeGrid[_currentMaze.Row][_currentMaze.Col].currentLocation = false;
                _mazeGrid[mc.Row - 1][mc.Col].currentLocation = true;
                _currentMaze = _mazeGrid[mc.Row-1][mc.Col];
                return new JsonResult(_mazeGrid);
              }
            return new JsonResult(false);
          }

          [HttpPost]
          public JsonResult GoSouth(MazeCell mc)
          {
            if (mc.Row + 1 >= _mazeGrid.Count)
            {
              return new JsonResult(false);
            }
            if (_mazeGrid[mc.Row + 1][mc.Col].Symbol == 'O' || _mazeGrid[mc.Row + 1][mc.Col].Symbol == 'S' || _mazeGrid[mc.Row + 1][mc.Col].Symbol == 'E')
            {
              _mazeGrid[_currentMaze.Row][_currentMaze.Col].currentLocation = false;
              _mazeGrid[mc.Row + 1][mc.Col].currentLocation = true;
              _currentMaze = _mazeGrid[mc.Row + 1][mc.Col];
              return new JsonResult(_mazeGrid);
            }
            return new JsonResult(false);
          }

          [HttpGet]
          public JsonResult getCurrentMaze()
          {
              return new JsonResult(_currentMaze);
          }

  }

}
