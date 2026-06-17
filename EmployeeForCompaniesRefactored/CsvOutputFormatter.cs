using System.Collections;
using System.Text;
using Entities;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Shared;

namespace EmployeeForCompaniesRefactored;

public class CsvOutputFormatter: TextOutputFormatter
{
    public CsvOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));    
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    public override bool CanWriteResult(OutputFormatterCanWriteContext context)
    {
        var objectType = context.ObjectType;
        if (typeof(CompanyDto).IsAssignableFrom(objectType) ||
            typeof(IEnumerable<CompanyDto>).IsAssignableFrom(objectType))
        {
            return base.CanWriteResult(context);
        }
        return false;
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;
        var buffer = new StringBuilder();
        if (context.Object is IEnumerable<CompanyDto> companies)
        {
            foreach (var company in (IEnumerable<CompanyDto>)context.Object)
            {
                FormatCsv(buffer, company);
            }
        }
        else FormatCsv(buffer, (CompanyDto)context.Object);
        await response.WriteAsync(buffer.ToString());
    }

    private static void FormatCsv(StringBuilder buffer, CompanyDto company)
    {
        buffer.AppendLine($"{company.Id}, {company.Name}, {company.FullAddress}");
    }
}