﻿using AttendanceSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.Repositories
{
    public interface ISelectedQuizQuestionRepository
    {
        Task AddSelectedQuestionsAsync(List<SelectedQuizQuestion> questions);
        Task<List<SelectedQuizQuestion>> GetAllSelectedQuestionsAsync();
        Task ClearSelectedQuestionsAsync();
    }
}
