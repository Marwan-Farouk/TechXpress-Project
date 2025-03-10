using MyProject.DataAccess;
using MyProject.DataAccess.DTOs;

namespace MyProject.Mappings
{
    public static class ProjectMappings
    {
        public static ProjectDTO ToDTO(this Project project)
        {
            return new ProjectDTO
            {
                Id = project.Id,
                Name = project.Name,
            };
        }
    }
}
