class App extends React.Component {
    render() {
        return (
            <table><thead>
                <tr><th>Name</th>
                    <th>Class</th>
                    <th>Section</th>
                </tr>
            </thead>

                <tbody>
                    <tr><td>Priya</td>
                        <td>I</td>
                        <td>a</td>
                    </tr>
                </tbody>


            </table>
        )
    }
}

React.render(<App />, document.getElementById("tableshow"));





