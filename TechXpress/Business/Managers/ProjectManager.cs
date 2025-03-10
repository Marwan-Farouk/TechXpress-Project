using MyProject.DataAccess.DTOs;
using System.Collections.Generic;

namespace MyProject.Bussiness.Managers
{
    public class ProjectManager : IProjectManager
    {
        private readonly List<ProjectDTO> _projects = new();

        public List<ProjectDTO> GetAllProjects()
        {
            return _projects;
        }

        public ProjectDTO GetProjectById(int id)
        {
            return _projects.Find(p => p.Id == id);
        }

        public void AddProject(ProjectDTO project)
        {
            _projects.Add(project);
        }

        public void UpdateProject(ProjectDTO project)
        {
            var existingProject = _projects.Find(p => p.Id == project.Id);
            if (existingProject != null)
            {
                existingProject.Name = project.Name;
                existingProject.Description = project.Description;
            }
        }

        public void DeleteProject(int id)
        {
            var project = _projects.Find(p => p.Id == id);
            if (project != null)
            {
                _projects.Remove(project);
            }
        }
    }
}