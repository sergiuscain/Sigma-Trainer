using DBLibrary.Data;
using DBLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sigma_Trainer.Services
{
    public class ExerciseService
    {
        private readonly SigmaTrainerDbContext _context;
        public ExerciseService(SigmaTrainerDbContext context)
        {
            _context = context;
        }
        public async Task AddExerciseAsync(Exercises exercises)
        {
            _context.Exercises.Add(exercises);
            _context.SaveChanges();
        }
        public async Task DeleteExerciseAsync(int exerciseID)
        {
            var existingExercise = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == exerciseID);
            if (existingExercise != null)
            {
                _context.Exercises.Remove(existingExercise);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Exercises> GetExerciseAsync(int exerciseID)
        {
            return await _context.Exercises.FirstOrDefaultAsync(e => e.Id == exerciseID);
        }
        public async Task<List<Exercises>> GetExercises()
        {
            return await _context.Exercises.ToListAsync();
        }
        public async Task UpdateExerciseAsync(Exercises exercises)
        {
            _context.Exercises.Update(exercises);
            await _context.SaveChangesAsync();
        }
    }
}