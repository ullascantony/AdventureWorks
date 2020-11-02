import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/layout';
import { Home } from './components/home';
import { Employees } from './components/employees';

import './custom.css'

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route exact path='/' component={Home} />
                <Route path='/employees' component={Employees} />
            </Layout>
        );
    }
}
