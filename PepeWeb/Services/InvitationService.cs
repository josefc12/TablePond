using AutoMapper;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using PepeWeb.Data;
using PepeWeb.Data.DTO;
using PepeWeb.Data.Models;
using System;
using System.Diagnostics;
using System.Security.Claims;

namespace PepeWeb.Services
{
    public class InvitationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private string? _userId;

        public InvitationService(ApplicationDbContext context, IMapper mapper, AuthenticationStateProvider authenticationStateProvider)
        {
            _context = context;
            _mapper = mapper;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> CheckCodeValidity(string? code)
        {

            if (code == null)
            {
                return false;
            }

            var invitation = await _context.Invitations.Where(c => c.Code == code && c.IsActive == true).FirstOrDefaultAsync();

            if (invitation != null)
            {
                return true;
            }
            return false;

        }

        public async Task<bool> CheckEmailValidity(string code, string email)
        {
            var invitation = await _context.Invitations.Where(c => c.Code == code && c.IsActive == true && c.EmailAddress == email).FirstOrDefaultAsync();

            if (invitation != null)
            {
                return true;
            }
            return false;

        }

        public async Task DeactivateCode(string email, string invitationCode)
        {
            Debug.WriteLine(email);
            Debug.WriteLine(invitationCode);
            // User already invited?
            var invitation = await _context.Invitations.Where(c => c.IsActive == true && c.EmailAddress == email && c.Code == invitationCode).FirstOrDefaultAsync();

            if (invitation != null)
            {
                invitation.IsActive = false;
                await _context.SaveChangesAsync();
            }

        }

        public async Task<List<InvitationDTO>> GetUserInvitations()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity?.IsAuthenticated == true)
            {
                _userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            return _mapper.Map<List<InvitationDTO>>(await _context.Invitations.Where(i => i.CreatedBy == _userId).ToListAsync());

        }

        public async Task<string?> InviteUser(string email)
        {
            // User already invited?
            var invitation = await _context.Invitations.Where(c => c.IsActive == true && c.EmailAddress == email).FirstOrDefaultAsync();

            if (invitation != null)
            {
                return null;
            }

            // Generate random string code
            string code = RandomString(8);

            try
            {
                Invitation newInvitation = new Invitation { Code = code, CreatedBy = _userId , EmailAddress = email, IsActive = true };
                _context.Invitations.Add(newInvitation);
                await _context.SaveChangesAsync();
                return code;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        private string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
