import { Stack, Spinner, SpinnerSize } from '@fluentui/react';

export function LoadingPanel() {
    return (<div style={{ marginTop: 20, marginLeft: 20, float: 'left' }}>
        <Spinner size={SpinnerSize.large} />
        <p style={{ color: '#fa8c16' }}>Loading...</p>
    </div>)
}