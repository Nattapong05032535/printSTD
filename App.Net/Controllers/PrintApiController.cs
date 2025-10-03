using App.Net.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace App.Net.Controllers
{
    [ApiController]
    [Route("api")]
    public class PrintApiController : ControllerBase
    {
        private readonly Server_API_Print _view;
        private readonly CashInData _cashInData;

        public PrintApiController(Server_API_Print view)
        {
            _view = view;
            _cashInData = new CashInData(0);
        }

        [HttpPost("print")]
        public async Task<IActionResult> Print([FromBody] RequestModel printModel)
        {
            try
            {
                string errorMessage = string.Empty;

                if (printModel == null || !printModel.Validate(out errorMessage))
                {
                    _view.Invoke((MethodInvoker)delegate
                    {
                        _view.HandleFrontendAndPrint(printModel ?? new RequestModel(), "Error: " + errorMessage);
                    });

                    LogErrorToFile("Validation Error", JsonSerializer.Serialize(printModel), errorMessage);
                    return BadRequest(new { status = "error", message = errorMessage });
                }

                _view.Invoke((MethodInvoker)delegate
                {
                    _view.HandleFrontendAndPrint(printModel, "Success");
                });

                decimal totalCashIn = printModel.CalculateTotalCashIn();
                _cashInData.TotalCashIn = totalCashIn;

                return Ok(new { status = "success", reqNo = printModel.ReqNo });
            }
            catch (JsonException ex)
            {
                LogErrorToFile("JSON Parsing Error", JsonSerializer.Serialize(printModel), ex.Message);
                return BadRequest(new { status = "error", message = "Invalid JSON format: " + ex.Message });
            }
            catch (Exception ex)
            {
                LogError($"Unexpected Error: {ex.Message}");
                return StatusCode(500, new { status = "error", message = "Unexpected error occurred." });
            }
        }

        [HttpPost("report")]
        public async Task<IActionResult> Report([FromBody] RequestReportModel reportModel)
        {
            try
            {
                string errorMessage = string.Empty;

                if (reportModel == null || !reportModel.Validate(out errorMessage))
                {
                    _view.Invoke((MethodInvoker)delegate
                    {
                        if (reportModel != null)
                        {
                            _view.HandleFrontendAndPrint(reportModel, "Error: " + errorMessage);
                        }
                        else
                        {
                            _view.HandleFrontendAndPrint(new RequestReportModel(), "Error: " + errorMessage);
                        }
                    });

                    LogErrorToFile("Validation Error", JsonSerializer.Serialize(reportModel), errorMessage);
                    return BadRequest(new { status = "error", message = errorMessage });
                }

                _view.Invoke((MethodInvoker)delegate
                {
                    _view.HandleFrontendAndPrint(reportModel, "Success");
                });

                return Ok(new { status = "success", message = "Report" });
            }
            catch (JsonException ex)
            {
                LogErrorToFile("JSON Parsing Error", JsonSerializer.Serialize(reportModel), ex.Message);
                return BadRequest(new { status = "error", message = "Invalid JSON format: " + ex.Message });
            }
            catch (Exception ex)
            {
                LogError($"Unexpected Error: {ex.Message}");
                return StatusCode(500, new { status = "error", message = "Unexpected error occurred." });
            }
        }

        private void LogError(string message)
        {
            string logMessage = $"Log Error while PrintController: {message}";
            LogToFile(logMessage);
        }

        private void LogErrorToFile(string errorType, string requestBody, string message)
        {
            string logMessage = $"{errorType} - {message}\nRequest Body: {requestBody}";
            LogToFile(logMessage);
        }

        private void LogToFile(string logMessage)
        {
            string filePath = Path.Combine(Application.StartupPath, "errorLog.txt");
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"Error Time: {DateTime.Now}");
                writer.WriteLine(logMessage);
                writer.WriteLine("----------------------------------------");
            }
        }
    }

    public class CashInData
    {
        public decimal TotalCashIn { get; set; }

        public CashInData(decimal initialTotalCashIn)
        {
            TotalCashIn = initialTotalCashIn;
        }
    }
}
