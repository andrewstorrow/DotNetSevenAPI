using DotNetSevenAPI.Data;
using DotNetSevenAPI.Dtos;
using DotNetSevenAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetSevenAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserJobInfoController : ControllerBase
{
    DataContextDapper _dapper;
    public UserJobInfoController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("GetUserJobInfos")]
    public IEnumerable<UserJobInfo> GetUserJobInfos()
    {
        string sql = @"
            SELECT 
                [UserId], 
                [JobTitle], 
                [Department] 
            FROM [TutorialAppSchema].[UserJobInfo]";

        IEnumerable<UserJobInfo> users = _dapper.LoadData<UserJobInfo>(sql);
        return users;
    }

    [HttpGet("GetSingleUserJobInfo/{userId}")]
    public UserJobInfo GetSingleUserJobInfo(int userId)
    {
        string sql = @"
            SELECT 
                [UserId], 
                [JobTitle], 
                [Department] 
            FROM [TutorialAppSchema].[UserJobInfo]
                WHERE UserId = " + userId.ToString();

        return _dapper.LoadDataSingle<UserJobInfo>(sql);
    }

    [HttpPut("EditUserJobInfo")]
    public IActionResult EditUserUserJobInfo(UserJobInfo userJobInfo)
    {
        string sql = @"
            UPDATE [TutorialAppSchema].[UserJobInfo]
                SET [JobTitle] = '" + userJobInfo.JobTitle +
                "', [Department] = '" + userJobInfo.Department +
                "' WHERE UserId = " + userJobInfo.UserId;

        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to Update UserJobInfo");
    }

    [HttpPost("AddUserJobInfo")]
    public IActionResult AddUserJobInfo(UserJobInfo userJobInfo)
    {
        string sql = @"
            INSERT INTO [TutorialAppSchema].[UserJobInfo](
                [UserId], 
                [JobTitle], 
                [Department]
            ) VALUES (" +
                "'" + userJobInfo.UserId +
                "', '" + userJobInfo.JobTitle +
                "', '" + userJobInfo.Department +
            "')";

        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to Add UserJobInfo");
    }

    [HttpDelete("DeleteUserJobInfo/{userId}")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        string sql = @"
        DELETE FROM [TutorialAppSchema].[UserJobInfo]
            WHERE UserId = " + userId.ToString();

        Console.WriteLine(sql);
        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to Delete UserJobInfo");
    }
}
