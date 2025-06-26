using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using App.Net.Model;

namespace App.Net.Controller
{
    public class PrintController
    {
        private Server_API_Print _view;
        private CashInData _cashInData;

        public PrintController(Server_API_Print view)
        {
            _view = view;
            _cashInData = new CashInData(0);
        }

        public virtual async Task ProcessRequest(HttpListenerContext context)
        {
            string requestBody;
            object? requestData = null;

            using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                requestBody = await reader.ReadToEndAsync();
            }

            var response = context.Response;
            var requestedUrl = context.Request?.Url?.AbsolutePath ?? string.Empty;

            try
            {
                if (requestedUrl.Contains("/api/print"))
                {
                    var printModel = JsonSerializer.Deserialize<RequestModel>(requestBody);
                    string errorMessage = string.Empty;

                    if (printModel == null || !printModel.Validate(out errorMessage))
                    {
                        SendErrorResponse(response, errorMessage);

                        _view.Invoke((MethodInvoker)delegate
                        {
                            _view.HandleFrontendAndPrint(printModel ?? new RequestModel(), "Error: " + errorMessage);
                        });

                        LogErrorToFile("Validation Error", requestBody, errorMessage);
                        return;
                    }

                    _view.Invoke((MethodInvoker)delegate
                    {
                        _view.HandleFrontendAndPrint(printModel, "Success");
                    });

                    decimal totalCashIn = printModel.CalculateTotalCashIn();
                    _cashInData.TotalCashIn = totalCashIn;

                    SendSuccessResponse(response, printModel.ReqNo);
                }
                else if (requestedUrl.Contains("/api/report"))
                {
                    // ประมวลผลการพิมพ์ (Print)
                    requestData = JsonSerializer.Deserialize<RequestReportModel>(requestBody);
                    string errorMessage = string.Empty;

                    // Validate the received data
                    var printModel = requestData as RequestReportModel;

                    if (printModel == null || !printModel.Validate(out errorMessage))
                    {
                        SendErrorResponse(response, errorMessage);

                        _view.Invoke((MethodInvoker)delegate
                        {
                            if (printModel != null)
                            {
                                _view.HandleFrontendAndPrint(printModel, "Error: " + errorMessage);
                            }
                            else
                            {
                                _view.HandleFrontendAndPrint(new RequestReportModel(), "Error: " + errorMessage);
                            }
                        });

                        LogErrorToFile("Validation Error", requestBody, errorMessage);
                        return;
                    }

                    _view.Invoke((MethodInvoker)delegate
                    {
                        _view.HandleFrontendAndPrint(printModel, "Success");
                    });

                    SendSuccessResponse(response, "Report");
                }
                else
                {
                    SendErrorResponse(response, "Unknown endpoint.");
                }
            }
            catch (JsonException ex)
            {
                LogErrorToFile("JSON Parsing Error", requestBody, ex.Message);

                SendErrorResponse(response, "Invalid JSON format: " + ex.Message);

                _view.Invoke((MethodInvoker)delegate
                {
                    if (requestData == null)
                    {
                        _view.HandleFrontendAndPrint(new RequestModel(), "Error: Invalid JSON format - " + ex.Message);
                    }
                    else
                    {
                        _view.HandleFrontendAndPrint(new RequestModel(), "Error: Invalid JSON format - " + ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                LogError($"Unexpected Error: {ex.Message}");
                SendErrorResponse(response, "Unexpected error occurred.");
            }
        }

        //public virtual async Task ProcessRequest(HttpListenerContext context)
        //{
        //    string requestBody;
        //    RequestModel? requestData = null;  

        //    using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
        //    {
        //        requestBody = await reader.ReadToEndAsync();
        //    }

        //    var response = context.Response;

        //    try
        //    {
        //        requestData = JsonSerializer.Deserialize<RequestModel>(requestBody);

        //        string errorMessage = string.Empty;

        //        // Validate the received data
        //        if (requestData == null || !requestData.Validate(out errorMessage))
        //        {
        //            SendErrorResponse(response, errorMessage);

        //            _view.Invoke((MethodInvoker)delegate
        //            {
        //                if (requestData != null)
        //                {
        //                    //MessageBox.Show(requestBody);
        //                    _view.HandleFrontendAndPrint(requestData, "Error: " + errorMessage);
        //                }
        //                else
        //                {
        //                    _view.HandleFrontendAndPrint(new RequestModel(), "Error: " + errorMessage);
        //                }
        //            });

        //            LogErrorToFile("Validation Error", requestBody, errorMessage);
        //            return;
        //        }

        //        _view.Invoke((MethodInvoker)delegate
        //        {
        //            _view.HandleFrontendAndPrint(requestData, "Success");
        //        });

        //        decimal totalCashIn = requestData.CalculateTotalCashIn();
        //        _cashInData.TotalCashIn = totalCashIn;

        //        SendSuccessResponse(response, requestData.ReqNo);
        //    }
        //    catch (JsonException ex)
        //    {
        //        LogErrorToFile("JSON Parsing Error", requestBody, ex.Message);

        //        SendErrorResponse(response, "Invalid JSON format: " + ex.Message);

        //        _view.Invoke((MethodInvoker)delegate
        //        {
        //            if (requestData == null)
        //            {
        //                _view.HandleFrontendAndPrint(new RequestModel(), "Error: Invalid JSON format - " + ex.Message);
        //            }
        //            else
        //            {
        //                _view.HandleFrontendAndPrint(requestData, "Error: Invalid JSON format - " + ex.Message);
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError($"Unexpected Error: {ex.Message}");
        //        SendErrorResponse(response, "Unexpected error occurred.");
        //    }
        //}

        private void SendSuccessResponse(HttpListenerResponse response, string reqNo)
        {
            try
            {
                var responseJson = JsonSerializer.Serialize(new { status = "success", idReceived = reqNo });
                byte[] buffer = Encoding.UTF8.GetBytes(responseJson);
                response.ContentType = "application/json";
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.OutputStream.Close();
            }
            catch (Exception ex)
            {
                LogError($"Error in SendSuccessResponse: {ex.Message}");
                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "SendSuccessResponse Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendErrorResponse(HttpListenerResponse response, string errorMessage)
        {
            try
            {
                var responseJson = JsonSerializer.Serialize(new { status = "error", message = errorMessage });
                byte[] buffer = Encoding.UTF8.GetBytes(responseJson);
                response.ContentType = "application/json";
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.OutputStream.Close();
            }
            catch (Exception ex)
            {
                LogError($"Error in SendErrorResponse: {ex.Message}");
                MessageBox.Show("กรุณาติดต่อเจ้าหน้าที่", "SendErrorResponse Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    public class ConcretePrintController : PrintController
    {
        public ConcretePrintController(Server_API_Print view) : base(view)
        {
            // การตั้งค่าหรือ logic ที่ต้องการ
        }

        public override async Task ProcessRequest(HttpListenerContext context)
        {
            // กำหนดการประมวลผลที่ต้องการ
            await base.ProcessRequest(context);  // เรียกใช้ method ของ PrintController ถ้าต้องการ
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
