import './App.css';
import ptBR from 'antd/es/locale/pt_BR';
import {ConfigProvider, theme} from "antd";
import React from "react";
import Home from './pages/Home';


function App() {
    // const {defaultAlgorithm, darkAlgorithm} = theme;
    //
    // const [isDarkMode, setIsDarkMode] = useState(true);
    // const handleClick = () => {
    //     setIsDarkMode((previousValue) => !previousValue);
    // };
    return (
        <ConfigProvider
            locale={ptBR}
            theme={{
                algorithm: theme.darkAlgorithm,
                token: {
                    colorPrimary: '#eb1946',
                    borderRadius: 20,
                },
            }}
        >
            {/*<Card style={{width: "max-content"}}>*/}
            {/*    <Button onClick={handleClick}>*/}
            {/*        Change Theme to {isDarkMode ? "Light" : "Dark"}*/}
            {/*    </Button>*/}
            {/*</Card>*/}
            <Home/>
        </ConfigProvider>
    );
}

export default App;
