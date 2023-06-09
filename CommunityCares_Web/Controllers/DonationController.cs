using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CommunityCares_Web.Data;
using CommunityCares_Web.Models;
using System.Net.Mail;
using System.Net;

namespace CommunityCares_Web.Controllers
{
    public class DonationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Donation
        public async Task<IActionResult> Index()
        {
            return _context.Donations != null ?
                       View(await _context.Donations.ToListAsync()) :
                       Problem("Entity set 'TopServiceBDOContext.CustomerUser'  is null.");
        }

        public IActionResult Index2(int id)
        {
           var don = from dona in _context.Donations.Include(a => a.IdDonnorsNavigation).Include(a => a.IdDonnorsNavigation.IdNavigation).Include(a => a.IdCampaignNavigation.IdAssylumNavigation)
                                             
                                             where dona.IdCampaign == id
                                             select dona;


            return View(don.ToList());
        }

        public IActionResult Index3(int id)
        {
            var don = from dona in _context.Donations.Include(a => a.IdDonnorsNavigation).Include(a => a.IdDonnorsNavigation.IdNavigation).Include(a => a.IdCampaignNavigation.IdAssylumNavigation)

                      where dona.IdCollect == id
                      orderby dona.Hour
                      select dona;


            return View(don.ToList());
        }

        public IActionResult DonacionesRecibidas(int id)
        {
            var don = from dona in _context.Donations.Include(a => a.IdDonnorsNavigation).Include(a => a.IdDonnorsNavigation.IdNavigation).Include(a => a.IdCampaignNavigation.IdAssylumNavigation)
                     
                      where dona.IdCampaign == id && dona.IsReceived == "Y"
                     
                      select dona;


            return View(don.ToList());
        }

        public IActionResult ConfirmarDonacion(int id)
        {
            var don = from dona in _context.Donations.Include(a => a.IdDonnorsNavigation).Include(a => a.IdDonnorsNavigation.IdNavigation).Include(a => a.IdCampaignNavigation.IdAssylumNavigation)

                      where dona.IdCampaign == id && dona.IsReceived == "N"
                      select dona;


            return View(don.ToList());
        }

        // GET: Donation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Donations == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .Include(d => d.IdCampaignNavigation)
                .Include(d => d.IdCollectsNavigation)
                .Include(d => d.IdDonnorsNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // GET: Donation/Create
        public IActionResult Create(int id)
        {
            ViewData["IdCampaign"] = new SelectList(_context.Campaigns, "Id", "Id");
            ViewData["IdCollect"] = new SelectList(_context.Collects.Where(x=>x.Id == id), "Id", "Date");
            ViewData["IdDonnors"] = new SelectList(_context.Donors, "Id", "Id");
            return View();
        }

        // POST: Donation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id,[Bind("DescriptionItems,DescriptionMonto,Status,RegisterDate,IsAnonymus,IsReceived,IdCollect,Hour,IdCampaign,IdDonnors")] Donation donation)
        {
            if (ModelState.IsValid)
            {
                donation.IsReceived = "N";
                donation.IdDonnors = SessionClass.Id;
                donation.IdCampaign = id;
                donation.RegisterDate = DateTime.Now;
                _context.Add(donation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index2", "Campaign");
            }
            ViewData["IdCampaign"] = new SelectList(_context.Campaigns, "Id", "Id", donation.IdCampaign);
            ViewData["IdCollect"] = new SelectList(_context.Collects, "Id", "Date", donation.IdCollect);
            ViewData["IdDonnors"] = new SelectList(_context.Donors, "Id", "Id", donation.IdDonnors);
            return RedirectToAction("Index2", "Campaign");
        }

        // GET: Donation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Donations == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound();
            }
            ViewData["IdCampaign"] = new SelectList(_context.Campaigns, "Id", "Id", donation.IdCampaign);
            ViewData["IdCollect"] = new SelectList(_context.Collects, "Id", "Id", donation.IdCollect);
            ViewData["IdDonnors"] = new SelectList(_context.Donors, "Id", "Id", donation.IdDonnors);
            return View(donation);
        }

        // POST: Donation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DescriptionItems,DescriptionMonto,Status,RegisterDate,IsAnonymus,IsReceived,IdCollect,Hour,IdCampaign,IdDonnors")] Donation donation)
        {
            if (id != donation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonationExists(donation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCampaign"] = new SelectList(_context.Campaigns, "Id", "Id", donation.IdCampaign);
            ViewData["IdCollect"] = new SelectList(_context.Collects, "Id", "Id", donation.IdCollect);
            ViewData["IdDonnors"] = new SelectList(_context.Donors, "Id", "Id", donation.IdDonnors);
            return View(donation);
        }

        //Metodo Para confirmar el recibo de la donacion
        

        public async Task<IActionResult> ConfirmarRecibo(int id)
        {

            var camp = await _context.Donations.FirstAsync(x => x.Id == id);
            var ID = await _context.Donations.FirstAsync(x => x.Id == id);
            var idCap = ID.IdDonnors;
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == idCap);

            camp.IsReceived = "Y";
            _context.Update(camp);
            await _context.SaveChangesAsync();


            EnviarCorreo(user.Email,"Des","cuerpo");



            return RedirectToAction("Index", "Campaign", new { id = ID.IdCampaign });
        }

        public void EnviarCorreo(string destinatario, string asunto, string cuerpo)
        {
            try
            {
                // Configurar el cliente SMTP
                SmtpClient clienteSmtp = new SmtpClient("smtp.gmail.com");
                clienteSmtp.Port = 587;
                clienteSmtp.EnableSsl = true;
                clienteSmtp.UseDefaultCredentials = false;
                clienteSmtp.Credentials = new NetworkCredential("rolycgr12@gmail.com", "iibbjmekeeoltgpv");

                // Crear el mensaje de correo
                MailMessage mensaje = new MailMessage();
                mensaje.From = new MailAddress("rolycgr12@gmail.com");
                mensaje.To.Add(destinatario);
                mensaje.Subject = asunto;
                mensaje.Body = cuerpo;

                // Enviar el correo
                clienteSmtp.Send(mensaje);
            }
            catch (Exception ex)
            {
                // Manejar cualquier error que pueda ocurrir durante el envío del correo
                Console.WriteLine("Error al enviar el correo: " + ex.Message);
            }
        }

        // GET: Donation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Donations == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .Include(d => d.IdCampaignNavigation)
                .Include(d => d.IdCollectsNavigation)
                .Include(d => d.IdDonnorsNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // POST: Donation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Donations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Donations'  is null.");
            }
            var donation = await _context.Donations.FindAsync(id);
            if (donation != null)
            {
                _context.Donations.Remove(donation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonationExists(int id)
        {
          return (_context.Donations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
