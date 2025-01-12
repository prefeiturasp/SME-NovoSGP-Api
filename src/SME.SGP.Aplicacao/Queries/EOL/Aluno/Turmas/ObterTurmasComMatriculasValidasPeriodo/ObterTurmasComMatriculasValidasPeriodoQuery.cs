﻿using MediatR;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComMatriculasValidasPeriodoQuery : IRequest<IEnumerable<string>>
    {
        public ObterTurmasComMatriculasValidasPeriodoQuery(string alunoCodigo, bool ehTurmaInfantil, int bimestre, long tipoCalendarioId, string[] turmasCodigos, DateTime periodoInicio, DateTime periodoFim, bool consideraPeriodoFechamento = true)
        {
            AlunoCodigo = alunoCodigo;
            EhTurmaInfantil = ehTurmaInfantil;
            Bimestre = bimestre;
            TipoCalendarioId = tipoCalendarioId;
            TurmasCodigos = turmasCodigos;
            PeriodoInicio = periodoInicio;
            PeriodoFim = periodoFim;
            ConsideraPeriodoFechamento = consideraPeriodoFechamento;
        }

        public string AlunoCodigo { get; set; }
        public string[] TurmasCodigos { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
        public long TipoCalendarioId { get; set; }
        public bool EhTurmaInfantil { get; set; }
        public int Bimestre { get; set; }
        public bool ConsideraPeriodoFechamento { get; set; }
    }
}
