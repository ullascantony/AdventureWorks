import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './nav-menu';

export class Layout extends Component {
    static displayName = Layout.name;

    render() {
        return (
            <div className="site">
                <NavMenu />
                <Container>
                    <div className="contents">
                        {this.props.children}
                    </div>
                    <div className="row">
                        <div className="col col-sm-12">
                            <div className="copyright text-center">&copy; 2020 UCA Software Solutions</div>
                        </div>
                    </div>
                </Container>
            </div>
        );
    }
}
