using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STA.LOG;
using STA.DOMAIN;
using STA.MODEL.Models;
using STA.REPOSITORY;
using STA.UI.WEB.Util;

namespace STA.UI.WEB.Controllers
{
    public class DeParaController : BaseController
    {
        //
        // GET: /DePara/

        [Autenticacao]
        public ActionResult Index()
        {
            Repository<TDEPARA> repository = new Repository<TDEPARA>();
            List<TDEPARA> list = repository.BuscarTodos().ToList();
            return View("Index", list);
        }
        [Autenticacao]
        public ActionResult Editar(int id)
        {
            DeParaDadosIniciais();
            Repository<TDEPARA> repository = new Repository<TDEPARA>();
            TDEPARA deParaModel = new TDEPARA();

            if (id != 0)
                deParaModel = repository.BuscarPorId(id);

            return View(deParaModel);
        }

        [HttpPost]
        public ActionResult Salvar(TDEPARA pModel)
        {
            try
            {
                if (String.IsNullOrEmpty(pModel.ANOM_ORIGEM) || String.IsNullOrEmpty(pModel.ANOM_DESTINO))
                {
                    ModelState.AddModelError("", "");
                    TempData["MessageErro"] = "Os campos ORIGEM e DESTINO são obrigatórios";
                    return View("Editar", pModel);
                }

                if (ModelState.IsValid)
                {
                    Repository<TDEPARA> repository = new Repository<TDEPARA>();

                    if (pModel.ANUM_DEPARA == 0)
                    {
                        repository.Adicionar(pModel);
                        TempData["MessageSucesso"] = "Registro inserido com sucesso";
                        return Redirect("/DePara/Editar/" + pModel.ANUM_DEPARA);
                    }
                    else
                    {
                        repository.Atualizar(pModel);
                        TempData["MessageSucesso"] = "Registro atualizado com sucesso";
                    }

                    return Redirect("~/DePara/Editar/" + pModel.ANUM_DEPARA);
                }
                else
                {
                    TempData["MessageErro"] = "Ocorreu um erro ao salvar";
                    return View("Editar", pModel);
                }
            }
            catch (DataException ex)
            {
                TempData["MessageErro"] = "Ocorreu um erro ao salvar";
                ModelState.AddModelError("", ex.ToString());
                return View("Editar", pModel);
            }

        }
        [Autenticacao]
        public ActionResult Excluir(int id)
        {
            Repository<TDEPARA> repository = new Repository<TDEPARA>();
            TDEPARA deParaModel = new TDEPARA();

            if (id != 0)
            {
                deParaModel = repository.BuscarPorId(id);
                repository.Remover(deParaModel);
                TempData["MessageSucesso"] = "Registro removido com sucesso";
            }
            return Redirect("/");
        }

        public ActionResult TestarLog()
        {
            Log.RegistrarLogInformacao("TESTE DE LOG DE INFORMAÇÃO REALIZADO COM SUCESSO");
            Log.RegistrarLogErro("TESTE DE LOG DE ERRO REALIZADO COM SUCESSO");
            return RedirectToAction("Index");
        }

        public ActionResult TestarDePara()
        {
            DeParaDomain depara = new DeParaDomain();
            depara.TransferirArquivosDePara();
            return RedirectToAction("Index");
        }
        
    }
}
