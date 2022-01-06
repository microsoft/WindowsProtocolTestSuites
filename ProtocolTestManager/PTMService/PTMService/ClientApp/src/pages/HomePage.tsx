// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Link, PrimaryButton } from '@fluentui/react'
import * as React from 'react'
import { FixedActionBar } from '../components/FixedActionBar'
import '../css/home.css'
import { useHistory } from 'react-router-dom'

const HomePage: React.FC = () => {
  const history = useHistory()

  return (
    <div className="ms-Grid home">
      <div className="ms-Grid-col ms-lg12 ms-xl11 ms-xxl11 ms-xxxl11">
        <div className="ms-Grid-row center">
          <div className="ms-Grid ms-Grid-col ms-lg12 ms-xl11 ms-xxl11">
            <p className="title">Protocol Test Manager Web Service</p>
          </div>

        </div>
        <div className="ms-Grid-row center">
          <div className="ms-Grid ms-Grid-col ms-lg12 ms-xl11 ms-xxl11">
            <div className="subTitle">A web application that can run protocol test suites and view test results</div>
          </div>
        </div>

        <div className="ms-Grid-row center">
          <div>
            <div className="buttons">
              <div className="panel" id="run-panel">
                <div className="content">
                  <PrimaryButton
                    className="run-button"
                    text="Run Test Suite"
                    onClick={() => { history.push('/Tasks/NewRun/', { from: 'HomePage' }) }}
                    allowDisabledFocus={true}
                  />
                  <p style={{ color: 'white' }}>Start to run test suite against SUT.</p>
                </div>
                <img className="robot-image" src='images/TS_Run.png' alt="PTM" />
              </div>
              <div className="panel" id="record-panel">
                <img className="robot-image" src="images/TS_History.png" alt="PTM" />
                <div className="content">
                  <PrimaryButton
                    className="record-button"
                    text="View Result"
                    onClick={() => history.push('/Tasks/History/', { from: 'HomePage' })}
                    allowDisabledFocus={true}
                  />
                  <p style={{ color: 'white' }}>View all previously Test Suite Result.</p>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="ms-Grid-row center">
          <div className="contact">
                      If you have any questions, please contact&nbsp;
                      <Link href="mailto:prototest@microsoft.com">prototest@microsoft.com</Link>.
                      <br /> Visit our GitHub repo at&nbsp;
                      <Link href="https://aka.ms/wpts">https://aka.ms/wpts</Link> for inquiry, issues or suggestions.
              </div>
        </div>
      </div>
      <div>
        <FixedActionBar >
          <div>
            <p>
              &copy; {new Date().getFullYear()} Microsoft &ensp;
          <a href="http://go.microsoft.com/fwlink/?LinkId=518021">Data Protection Notice</a> &ensp;
          <a href="https://go.microsoft.com/fwlink/?LinkID=206977">Terms of use</a> &ensp;
          <a href="https://www.microsoft.com/trademarks">Trademarks</a>
            </p>
          </div>

        </FixedActionBar>
      </div>
    </div >
  )
}

export default HomePage
