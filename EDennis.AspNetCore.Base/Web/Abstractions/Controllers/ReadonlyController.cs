﻿using DevExtreme.AspNet.Data;
using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ReadonlyController<TEntity, TContext> : ControllerBase
            where TEntity : class, new()
            where TContext : DbContext {

        private readonly ReadonlyRepo<TEntity,TContext> _repo;

        public ReadonlyController(ReadonlyRepo<TEntity, TContext> repo) {
            _repo = repo;
        }

        /// <summary>
        /// Get from OData query string
        /// </summary>
        /// <returns></returns>
        [EnableQuery]
        [HttpGet("odata")]
        public IEnumerable<TEntity> GetOData() {
            return _repo.Query;
        }


        /// <summary>
        /// Get from DevExtreme DataSourceLoader query string
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <returns></returns>
        [HttpGet("devextreme")]
        public IActionResult GetDevExtreme(
                [FromQuery]string select,
                [FromQuery]string sort,
                [FromQuery]string filter,
                [FromQuery]int skip,
                [FromQuery]int take,
                [FromQuery]string totalSummary,
                [FromQuery]string group,
                [FromQuery]string groupSummary
            ) {
            var loadOptions = DataSourceLoadOptionsBuilder.Build(
                select, sort, filter, skip, take, totalSummary,
                group, groupSummary);

            var result = DataSourceLoader.Load(_repo.Query, loadOptions);
            return Ok(result);
        }


        /// <summary>
        /// Get by Dynamic Linq Expression
        /// https://github.com/StefH/System.Linq.Dynamic.Core
        /// https://github.com/StefH/System.Linq.Dynamic.Core/wiki/Dynamic-Expressions
        /// </summary>
        /// <param name="where">string Where expression</param>
        /// <param name="orderBy">string OrderBy expression (with support for descending)</param>
        /// <param name="select">string Select expression</param>
        /// <param name="skip">int number of records to skip</param>
        /// <param name="take">int number of records to return</param>
        /// <returns>dynamic-typed object</returns>
        [HttpGet("dynamic")]
        public IActionResult GetDynamicLinq(
                [FromQuery]string where = null,
                [FromQuery]string orderBy = null,
                [FromQuery]string select = null,
                [FromQuery]int? skip = null,
                [FromQuery]int? take = null
                ) {
            return new ObjectResult(_repo.GetFromDynamicLinq(
                where, orderBy, select, skip, take));
        }


        /// <summary>
        /// Get by Dynamic Linq Expression
        /// https://github.com/StefH/System.Linq.Dynamic.Core
        /// https://github.com/StefH/System.Linq.Dynamic.Core/wiki/Dynamic-Expressions
        /// </summary>
        /// <param name="where">string Where expression</param>
        /// <param name="orderBy">string OrderBy expression (with support for descending)</param>
        /// <param name="select">string Select expression</param>
        /// <param name="skip">int number of records to skip</param>
        /// <param name="take">int number of records to return</param>
        /// <returns>dynamic-typed object</returns>
        [HttpGet("dynamic/async")]
        public async Task<IActionResult> GetDynamicLinqAsync(
                [FromQuery]string where = null,
                [FromQuery]string orderBy = null,
                [FromQuery]string select = null,
                [FromQuery]int? skip = null,
                [FromQuery]int? take = null
                ) {
            return new ObjectResult(await _repo.GetFromDynamicLinqAsync(
                where, orderBy, select, skip, take));
        }



        [HttpGet("sp")]
        public IActionResult GetFromStoredProcedure(
            StoredProcedureWithParameters storedProcedureWithParameters) {
            return Ok(_repo.GetFromStoredProcedure(
                storedProcedureWithParameters.StoredProcedureName,
                storedProcedureWithParameters.Parameters));
        }

        [HttpGet("sp/async")]
        public async Task<IActionResult> GetFromStoredProcedureAsync(
            StoredProcedureWithParameters storedProcedureWithParameters) {
            var result = await _repo.GetFromStoredProcedureAsync(
                storedProcedureWithParameters.StoredProcedureName,
                storedProcedureWithParameters.Parameters);
            return Ok(result);
        }


        [HttpGet("json")]
        public IActionResult GetJsonColumnFromStoredProcedure(
            StoredProcedureWithParameters storedProcedureWithParameters) {
            return Ok(_repo.GetJsonColumnFromStoredProcedure(
                storedProcedureWithParameters.StoredProcedureName,
                storedProcedureWithParameters.Parameters));
        }

        [HttpGet("json/async")]
        public async Task<IActionResult> GetJsonColumnFromStoredProcedureAsync(
            StoredProcedureWithParameters storedProcedureWithParameters) {
            var result = await _repo.GetJsonColumnFromStoredProcedureAsync(
                storedProcedureWithParameters.StoredProcedureName,
                storedProcedureWithParameters.Parameters);
            return Ok(result);
        }

    }
}
