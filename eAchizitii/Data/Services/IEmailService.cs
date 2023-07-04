using eAchizitii.Data.ViewModels;
using eAchizitii.Models;
using Microsoft.AspNetCore.Identity;

namespace eAchizitii.Data.Services
{
    public interface IEmailService
    {

        bool TrimiteMail(string to, string from, string subject, string body);

    }
}
