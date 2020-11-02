import React, { Component } from 'react';

export class Employees extends Component {
    static displayName = Employees.name;

    constructor(props) {
        super(props);
        this.rows = 10;
        this.state = {
            employees: [],
            processing: true,
            page: 1,
            records: 0,
            pagerDesc: ''
        };
    }

    componentDidMount() {
        this.populateData(this.state.page);
    }

    renderTable() {
        return (
            <div className="table-container">
                <table className='table table-site table-striped' aria-labelledby="table-label">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Designation</th>
                            <th>Email</th>
                            <th>Phone</th>
                            <th>City</th>
                            <th>Country</th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.state.employees.map(emp =>
                            <tr key={emp.BusinessEntityID}>
                                <td>{emp.FirstName + ' ' + emp.LastName}</td>
                                <td>{emp.JobTitle}</td>
                                <td>{emp.EmailAddress}</td>
                                <td>{emp.PhoneNumber}</td>
                                <td>{emp.City}</td>
                                <td>{emp.CountryRegionName}</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        );
    }

    renderPaging() {
        return (
            <nav aria-label="Pagination">
                <ul className="pagination pagination-site">
                    <li className={this.state.processing || this.state.page <= 1 ? 'page-item disabled' : 'page-item'} title="Go to first page">
                        <span className="page-link" onClick={(e) => { this.goToPage(e, 'first') }}>|&lt;&lt;</span>
                    </li>
                    <li className={this.state.processing || this.state.page <= 1 ? 'page-item disabled' : 'page-item'} title={this.state.page > 1 ? 'Go to page ' + (this.state.page - 1) : ''}>
                        <span className="page-link" onClick={(e) => { this.goToPage(e, 'prev') }}>&lt;&lt;</span>
                    </li>
                    <li className={this.state.processing || this.state.page * this.rows >= this.state.records ? 'page-item disabled' : 'page-item'} title={this.state.page * this.rows < this.state.records ? 'Go to page ' + (this.state.page + 1) : ''}>
                        <span className="page-link" onClick={(e) => { this.goToPage(e, 'next') }}>&gt;&gt;</span>
                    </li>
                    <li className={this.state.processing || this.state.page * this.rows >= this.state.records ? 'page-item disabled' : 'page-item'} title="Go to last page">
                        <span className="page-link" onClick={(e) => { this.goToPage(e, 'last') }}>&gt;&gt;|</span>
                    </li>
                </ul>
                <span className="pagination-descripton" title={'Showing ' + this.state.pagerDesc + ' records'}>{this.state.pagerDesc}</span>
            </nav>
        );
    }

    render() {
        let contents = this.state.processing
            ? <div className="alert alert-info alert-site" role="alert">Processing, please wait...</div>
            : this.renderTable();

        let paging = this.state.processing
            ? null
            : this.renderPaging();

        return (
            <div className="row">
                <div className="col col-sm-12">
                    <div className="card card-site card-site-dark">
                        <div className="card-header">Employees</div>
                        <div className="card-body">
                            <div className="card-text">
                                {contents}
                                {paging}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    goToPage(e, direction) {
        if (e) {
            e.preventDefault();
        }
        var page = this.state.page;
        if (direction === 'first' && page > 1) {
            page = 1;
            this.populateData(page);
        }
        else if (direction === 'prev' && page > 1) {
            page--;
            this.populateData(page);
        }
        else if (direction === 'next' && page * this.rows < this.state.records) {
            page++;
            this.populateData(page);
        }
        else if (direction === 'last' && page * this.rows < this.state.records) {
            page = Math.floor(this.state.records / this.rows);
            this.populateData(page);
        }
    }

    async populateData(page) {
        var url = 'api/employee/getall/' + this.rows + '/' + (page - 1);
        const response = await fetch(url);
        const data = await response.json();
        if (Array.isArray(data) && data.length) {
            let records = data.map(emp => emp.ResultCount)[0];
            let pagerDesc = (((page - 1) * this.rows) + 1) + ' to ' + (page * this.rows) + ' of ' + records;
            this.setState({
                employees: data,
                processing: false,
                records: records,
                page: page,
                pagerDesc: pagerDesc
            });
        }
    }
}
