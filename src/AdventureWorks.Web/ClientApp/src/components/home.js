import React, { Component } from 'react';

export class Home extends Component {
    static displayName = Home.name;

    componentDidMount() {
        document.title = 'AdventureWorks | Home';
    }

    render() {
        return (
            <div className="row">
                <div className="col-sm-12">
                    <div className="card card-site card-site-dark">
                        <div className="card-header">React SPA</div>
                        <div className="card-body">
                            <div className="card-text">
                                <h2>Developer</h2>
                                <ul type="none">
                                    <li><a href="https://www.google.com/search?q=Ullas+Chacko+Antony" rel="noopener noreferrer" target="_blank">Ullas Chacko ANTONY</a></li>
                                </ul>
                                <h2>Database</h2>
                                <ul>
                                    <li><a href="https://www.microsoft.com/en-us/sql-server" rel="noopener noreferrer" target="_blank">Microsoft SQL Server</a> RDBMS for data persistence.</li>
                                    <li><a href="https://docs.microsoft.com/en-us/sql/samples/adventureworks-install-configure" rel="noopener noreferrer" target="_blank">AdventureWorks</a> sample database as data source.</li>
                                </ul>
                                <h2>UX design &amp; frameworks</h2>
                                <ul>
                                    <li><a href="https://get.asp.net/" rel="noopener noreferrer" target="_blank">ASP.NET Core</a> and <a href="https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx" rel="noopener noreferrer" target="_blank">C#</a> for cross-platform server-side code.</li>
                                    <li><a href="https://reactjs.org/" rel="noopener noreferrer" target="_blank">React</a> for client-side code.</li>
                                    <li><a href="http://getbootstrap.com/" rel="noopener noreferrer" target="_blank">Bootstrap</a> for responsive layout and styling.</li>
                                </ul>
                                <h2>Application development features</h2>
                                <ul>
                                    <li><strong>Client-side navigation</strong>. For example, click <em>Employees</em> then <em>Back</em> to return here.</li>
                                    <li><strong>Development server integration</strong>. In development mode, the development server from <code>create-react-app</code> runs in the background automatically, so your client-side resources are dynamically built on demand and the page refreshes when you modify any file.</li>
                                    <li><strong>Efficient production builds</strong>. In production mode, development-time features are disabled, and your <code>dotnet publish</code> configuration produces minified, efficiently bundled JavaScript files.</li>
                                </ul>
                                The <code>ClientApp</code> subdirectory is a standard React application based on the <code>create-react-app</code> template. If you open a command prompt in that directory, you can run <code>npm</code> commands such as <code>npm test</code> or <code>npm install</code>.
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}
