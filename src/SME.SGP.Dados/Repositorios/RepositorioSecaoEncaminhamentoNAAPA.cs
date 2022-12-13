﻿using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSecaoEncaminhamentoNAAPA : RepositorioBase<SecaoEncaminhamentoNAAPA>, IRepositorioSecaoEncaminhamentoNAAPA
    {
        public RepositorioSecaoEncaminhamentoNAAPA(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
            
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> ObterSecaoEncaminhamentoDtoPorEtapa(List<int> etapas, long? encaminhamentoNAAPAId)
        {
            var query = @"SELECT sea.id
	                            , sea.nome
	                            , sea.questionario_id as questionarioId
	                            , eas.concluido
	                            , sea.etapa
                                , sea.ordem
                                , sea.nome_componente as nomeComponente
                         FROM secao_encaminhamento_naapa sea
                        left join encaminhamento_naapa_secao eas on eas.encaminhamento_naapa_id = @encaminhamentoNAAPAId and eas.secao_encaminhamento_id = sea.id
                         WHERE not sea.excluido 
                           AND sea.etapa = ANY(@etapas)
                         ORDER BY sea.etapa, sea.ordem ";

            return await database.Conexao.QueryAsync<SecaoQuestionarioDto>(query, new { etapas, encaminhamentoNAAPAId = encaminhamentoNAAPAId ?? 0 });
        }

        public async Task<IEnumerable<SecaoEncaminhamentoNAAPA>> ObterSecoesEncaminhamentoPorEtapaModalidade(List<int> etapas, int modalidade, long? encaminhamentoNAAPAId)
        {
            var query = new StringBuilder(@"SELECT sea.*, eas.*
                                            FROM secao_encaminhamento_naapa sea 
                                                left join encaminhamento_naapa_secao eas on eas.encaminhamento_naapa_id = @encaminhamentoNAAPAId
                                                                                        and eas.secao_encaminhamento_id = sea.id
                                                left join secao_encaminhamento_naapa_modalidade senm on senm.secao_encaminhamento_id = sea.id 
                                            WHERE not sea.excluido AND sea.etapa = ANY(@etapas) 
                                                  AND ((senm.modalidade_codigo = @modalidade) or (senm.modalidade_codigo is null)) 
                                            ORDER BY sea.etapa, sea.ordem; ");

            return await database.Conexao
                .QueryAsync<SecaoEncaminhamentoNAAPA, EncaminhamentoNAAPASecao, SecaoEncaminhamentoNAAPA>(
                    query.ToString(), (secaoEncaminhamento, encaminhamentoSecao) =>
                    {
                        secaoEncaminhamento.EncaminhamentoNAAPASecao = encaminhamentoSecao;
                        return secaoEncaminhamento;
                    }, new { etapas, encaminhamentoNAAPAId = encaminhamentoNAAPAId ?? 0, modalidade });
        }
    }
}