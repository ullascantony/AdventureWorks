import React, { Component } from 'react';

export class Employees extends Component {
    static displayName = Employees.name;

    constructor(props) {
        super(props);
        this.state = {
            employees: [],
            processing: true,
            pager: {
                totalItems: 0,
                currentPage: 0,
                pageSize: 10,
                totalPages: 0,
                startPage: 0,
                endPage: 0,
                startIndex: 0,
                endIndex: 0,
                pages: [],
                pagerSummary: ''
            }
        };
    }

    componentDidMount() {
        document.title = 'AdventureWorks | Employees';
        this.populateData(1);
    }

    getRange(start, end, step) {
        const range = [];
        if (step === null || typeof (step) === 'undefined') {
            step = 1;
        }
        if (end < start) {
            step = -step;
        }
        while (step > 0 ? end >= start : end <= start) {
            range.push(start);
            start += step;
        }
        return range;
    }

    getPager(totalItems, currentPage = 1, pageSize = 10, typeName = 'row', typeNamePlural = 'rows') {
        // Calculate total pages
        const totalPages = Math.ceil(totalItems / pageSize);

        var startPage = 0, endPage = 0;

        if (totalPages <= 10) {
            // Less than 10 total pages so show all
            startPage = 1;
            endPage = totalPages;
        } else {
            // More than 10 total pages so calculate start and end pages
            if (currentPage <= 6) {
                startPage = 1;
                endPage = 10;
            } else if (currentPage + 4 >= totalPages) {
                startPage = totalPages - 9;
                endPage = totalPages;
            } else {
                startPage = currentPage - 5;
                endPage = currentPage + 4;
            }
        }

        // Calculate start and end item indexes
        const startIndex = (currentPage - 1) * pageSize;
        const endIndex = Math.min(startIndex + pageSize - 1, totalItems - 1);

        // Create an array of available pages
        const pages = this.getRange(startPage, endPage, 1);

        // Compose pager summary
        let pagerSummary = (startIndex + 1) + ' ';
        pagerSummary += ((startIndex + 1) !== (endIndex + 1) ? 'to' : '') + ' ';
        pagerSummary += ((startIndex + 1) !== (endIndex + 1) ? (endIndex + 1 + '') : '') + ' ';
        pagerSummary += 'of ' + totalItems + ' ' + (totalItems === 1 ? typeName : typeNamePlural);

        return {
            totalItems: totalItems,
            currentPage: currentPage,
            pageSize: pageSize,
            totalPages: totalPages,
            startPage: startPage,
            endPage: endPage,
            startIndex: startIndex,
            endIndex: endIndex,
            pages: pages,
            pagerSummary: pagerSummary
        };
    }

    goToPage(e, page) {
        if (e) {
            e.preventDefault();
        }
        this.populateData(page);
    }

    async populateData(page) {
        const url = 'api/employee/getall/' + this.state.pager.pageSize + '/' + (page - 1);
        const response = await fetch(url);
        const data = await response.json();
        if (Array.isArray(data) && data.length) {
            let records = data.map(emp => emp.ResultCount)[0];
            const pager = this.getPager(records, page, this.state.pager.pageSize, 'employee', 'employees');
            this.setState({
                employees: data,
                processing: false,
                pager: pager
            });
        }
    }

    renderTable() {
        return (
            <div className="table-container">
                <table className='table table-site' aria-labelledby="table-label">
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
                <ul className="pagination pagination-site" role="navigation">
                    <li className={this.state.processing || this.state.pager.currentPage <= 1 ? 'page-item disabled' : 'page-item'} title={this.state.pager.currentPage > 1 ? 'Go to first page' : ''}>
                        <span className="page-link" onClick={(e) => { this.goToPage(e, 1) }}>|&lt;</span>
                    </li>
                    <li className={this.state.processing || this.state.pager.currentPage <= 1 ? 'page-item disabled' : 'page-item'} title={this.state.pager.currentPage > 1 ? 'Go to previous page (' + (this.state.pager.currentPage - 1) + ')' : ''}>
                        <span className="page-link" onClick={(e) => { this.goToPage(e, this.state.pager.currentPage - 1) }}>&lt;&lt;</span>
                    </li>
                    {this.state.pager.pages.map(page =>
                        <li key={page} className={this.state.processing || this.state.pager.currentPage === page ? 'page-item current' : 'page-item'} title={this.state.pager.currentPage !== page ? 'Go to page ' + page : 'Current page is ' + page}>
                            <span className="page-link" onClick={(e) => { this.goToPage(e, page) }}>{page}</span>
                        </li>
                    )}
                    <li className={this.state.processing || this.state.pager.currentPage >= this.state.pager.totalPages ? 'page-item disabled' : 'page-item'} title={this.state.pager.currentPage < this.state.pager.totalPages ? 'Go to next page (' + (this.state.pager.currentPage + 1) + ')' : ''}>
                        <span className="page-link" onClick={(e) => { this.goToPage(e, this.state.pager.currentPage + 1) }}>&gt;&gt;</span>
                    </li>
                    <li className={this.state.processing || this.state.pager.currentPage >= this.state.pager.totalPages ? 'page-item disabled' : 'page-item'} title={this.state.pager.currentPage < this.state.pager.totalPages ? 'Go to last page (' + (this.state.pager.totalPages) + ')' : ''}>
                        <span className="page-link" onClick={(e) => { this.goToPage(e, this.state.pager.totalPages) }}>&gt;|</span>
                    </li>
                </ul>
                <span className="pagination-summary" title={this.state.pager.pagerSummary}>{this.state.pager.pagerSummary}</span>
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
                <div className="col-sm-12">
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
}
