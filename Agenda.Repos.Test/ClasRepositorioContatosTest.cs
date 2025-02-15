﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agenda.DAL;
using Agenda.Repos;
using Agenda.Domain;
using NUnit.Framework;
using Moq;

namespace Agenda.Repos.Test
{
    [TestFixture]
    public class ClasRepositorioContatosTest
    {
        Mock<IContatos> _contatos;
        Mock<ITelefones> _telefones;
        RepositorioContatos _repositorioContatos;

        [SetUp]
        public void Setup()
        {
            _contatos = new Mock<IContatos>();
            _telefones = new Mock<ITelefones>();
            _repositorioContatos = new RepositorioContatos(_contatos.Object, _telefones.Object);
        }

        [Test]
        public void DeveSerPossivelObterContatoComListaTelefone()
        {
            Guid telefoneId = Guid.NewGuid();
            Guid contatoId = Guid.NewGuid();
            List<ITelefone> lstTelefone = new List<ITelefone>();
            //Monta
            //Criar Moq de IContato
            Mock<IContato> mContato = IContatoConstr.Um().ComId(contatoId).ComNome("João").Obter();
            //mContato.SetupGet(o => o.Id).Returns(contatoId);
            //mContato.SetupGet(o => o.Nome).Returns("João");
            mContato.SetupSet(o => o.Telefones = It.IsAny<List<ITelefone>>())
                .Callback<List<ITelefone>>(p => lstTelefone = p);
            //Moq da função ObterPorId de IContatos
            _contatos.Setup(o => o.Obter(contatoId)).Returns(mContato.Object);
            //Criar Moq de Itelefone
            ITelefone mockTelefone = ITelefoneConstr
                .Um()
                .Padrao()
                .ComId(telefoneId)
                .ComContatoId(contatoId)
                .Construir();
            //Moq da função ObterTodosDoContato de ITelefones 
            _telefones.Setup(o => o.ObterTodosDoContato(contatoId)).Returns(new List<ITelefone> { mockTelefone });
            //Excuta
            //chama o metodo ObterPorId de RepositorioContatos
            IContato contatoResultado = _repositorioContatos.ObterPorId(contatoId);
            mContato.SetupGet(o => o.Telefones).Returns(lstTelefone);
            //Verifica
            //Verificar se o Conato retornado contém os mesmos dados do Moq Icontato com a lista de Telefones do Moq Itelefone
            Assert.AreEqual(mContato.Object.Id, contatoResultado.Id);
            Assert.AreEqual(mContato.Object.Nome, contatoResultado.Nome);
            Assert.AreEqual(1, contatoResultado.Telefones.Count);
            Assert.AreEqual(mockTelefone.Numero, contatoResultado.Telefones[0].Numero);
            Assert.AreEqual(mockTelefone.Id, contatoResultado.Telefones[0].Id);
            Assert.AreEqual(mContato.Object.Id, contatoResultado.Telefones[0].ContatoId);
        }

        [TearDown]
        public void TearDown()
        {
            _contatos = null;
            _telefones = null;

        }

    }
}
