import { PrimaryButton, Stack } from '@fluentui/react'
import './../css/index.css'

export interface TestSuiteInfoProp {
  TestSuiteName: string
  Description?: string
  Version?: string
  OnSelect: () => void
}

export function TestSuiteInfo (props: TestSuiteInfoProp) {
  return (<div className="card">
        <Stack className="container">
            <div>
                <div className="subject">{`${props.TestSuiteName} - (${props.Version})`}</div>
                <div className="description">{props.Description}</div>
            </div>
            <Stack horizontal horizontalAlign="end">
                <PrimaryButton onClick={() => { props.OnSelect() }}>Select</PrimaryButton>
            </Stack>
        </Stack>

    </div>)
}
