using MyProject.DataAccess;
using MyProject.DataAccess.DTOs;
using MyProject.Mappings;

namespace MyProject.Business.Services;

public class ProjectService
{
    public List<ProjectDTO> GetProjects()
    {
        var projects = new List<Project>
        {
            new Project { Id = 1, Name = "Project 1", Description = "Description 1" },
            new Project { Id = 2, Name = "Project 2", Description = "Description 2" }
        };

        return projects.Select(p => p.ToDTO()).ToList()
    }
}
