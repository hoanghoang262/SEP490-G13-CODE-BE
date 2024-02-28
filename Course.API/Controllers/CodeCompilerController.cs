﻿using CompileCodeOnline;
using CourseService.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using System.Reflection;


namespace DynamicCodeCompilerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CodeCompilerController : ControllerBase
    {
        private readonly DynamicCodeCompiler _codeCompiler;
        private readonly CourseContext _context;

        public CodeCompilerController(DynamicCodeCompiler codeCompiler, CourseContext context)
        {
            _codeCompiler = codeCompiler;
            _context = context;
        }

        [HttpPost]
        public IActionResult CompileAndRunCode( CodeRequestModel request)
        {
            var testCases = from codeQuestion in _context.CodeQuestions
                            join testCase in _context.TestCases
                            on codeQuestion.Id equals testCase.CodeQuestionId
                            where testCase.CodeQuestionId == 1
                            select new
                            {
                                codeQuestion.Description,
                                testCase.ExpectedResultInt,
                                testCase.InputTypeInt,
                                testCase.InputTypeString,
                                testCase.ExpectedResultString,
                                testCase.InputTypeArrayInt,
                                testCase.InputTypeBoolean,
                                testCase.ExpectedResultBoolean,
                                testCase.InputTypeArrayString
                            };


            try
            {

                string generatedCode = _codeCompiler.GenerateCode("Solution", request.UserCode);


                Assembly assembly = _codeCompiler.CompileCode(generatedCode);

                // Perform tests
                foreach (var testCase in testCases)
                {
                    int[] inputArray;
                    if (!string.IsNullOrEmpty(testCase.InputTypeArrayInt))
                    {
                        try
                        {
                            var stringInput = testCase.InputTypeArrayInt.Split(',');
                            inputArray = stringInput.Select(item => int.Parse(item)).ToArray();
                            object result = _codeCompiler.InvokeMethod(assembly, "Solution", "Answer", inputArray);

                            if (testCase.ExpectedResultInt.HasValue)
                            {
                                if (!result.Equals(testCase.ExpectedResultInt))
                                {
                                    return BadRequest($"Test failed for input '{testCase.InputTypeArrayInt}': Expected result '{testCase.ExpectedResultInt}' does not match actual result '{result}'.");
                                }

                            }
                            else if (!string.IsNullOrEmpty(testCase.ExpectedResultString))
                            {
                                if (!result.Equals(testCase.ExpectedResultString))
                                {
                                    return BadRequest($"Test failed for input '{testCase.InputTypeArrayInt}': Expected result '{testCase.ExpectedResultString}' does not match actual result '{result}'.");
                                }
                            }
                            else if (testCase.ExpectedResultBoolean.HasValue)
                            {
                                if (!result.Equals(testCase.ExpectedResultBoolean))
                                {
                                    return BadRequest($"Test failed for input '{testCase.InputTypeArrayInt}': Expected result '{testCase.ExpectedResultString}' does not match actual result '{result}'.");
                                }
                            }

                        }
                        catch (Exception e)
                        {

                            throw;
                        }
                    }
                    if (!string.IsNullOrEmpty(testCase.InputTypeArrayString))
                    {
                        try
                        {
                            var stringInput = testCase.InputTypeArrayInt.Split(',');

                            object result = _codeCompiler.InvokeMethod(assembly, "Solution", "Answer", stringInput);

                            if (testCase.ExpectedResultInt.HasValue)
                            {
                                if (!result.Equals(testCase.ExpectedResultInt))
                                {
                                    return BadRequest($"Test failed for input '{testCase.InputTypeArrayString}': Expected result '{testCase.ExpectedResultInt}' does not match actual result '{result}'.");
                                }

                            }
                            else if (!string.IsNullOrEmpty(testCase.ExpectedResultString))
                            {
                                if (!result.Equals(testCase.ExpectedResultString))
                                {
                                    return BadRequest($"Test failed for input '{testCase.InputTypeArrayString}': Expected result '{testCase.ExpectedResultString}' does not match actual result '{result}'.");
                                }
                            }
                            else if (testCase.ExpectedResultBoolean.HasValue)
                            {
                                if (!result.Equals(testCase.ExpectedResultBoolean))
                                {
                                    return BadRequest($"Test failed for input '{testCase.InputTypeArrayString}': Expected result '{testCase.ExpectedResultString}' does not match actual result '{result}'.");
                                }
                            }

                        }
                        catch (Exception e)
                        {

                            throw;
                        }
                    }

                    if (testCase.ExpectedResultInt.HasValue || string.IsNullOrEmpty(testCase.ExpectedResultString) || testCase.InputTypeBoolean.HasValue)
                    {
                        object inputValue;
                        if (testCase.InputTypeInt.HasValue)
                        {
                            inputValue = testCase.InputTypeInt.Value;
                        }
                        else if (!string.IsNullOrEmpty(testCase.InputTypeString))
                        {
                            inputValue = testCase.InputTypeString;
                        }
                        else
                        {

                            continue;
                        }
                        object result = _codeCompiler.InvokeMethod(assembly, "Solution", "Answer", inputValue);

                        if (testCase.ExpectedResultInt.HasValue)
                        {
                            if (!result.Equals(testCase.ExpectedResultInt))
                            {
                                return BadRequest($"Test failed for input '{inputValue}': Expected result '{testCase.ExpectedResultInt}' does not match actual result '{result}'.");
                            }

                        }
                        else if (!string.IsNullOrEmpty(testCase.ExpectedResultString))
                        {
                            if (!result.Equals(testCase.ExpectedResultString))
                            {
                                return BadRequest($"Test failed for input '{inputValue}': Expected result '{testCase.ExpectedResultString}' does not match actual result '{result}'.");
                            }
                        }
                        else if (testCase.ExpectedResultBoolean.HasValue)
                        {
                            if (!result.Equals(testCase.ExpectedResultBoolean))
                            {
                                return BadRequest($"Test failed for input '{inputValue}': Expected result '{testCase.ExpectedResultString}' does not match actual result '{result}'.");
                            }
                        }


                    }

                }


                return Ok("All tests passed!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Argument error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }


}

public class CodeRequestModel
{
    public string UserCode { get; set; }
}


