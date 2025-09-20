import { FluentProvider, webLightTheme } from '@fluentui/react-components';
import 'bootstrap/dist/css/bootstrap.min.css';
import './Styles/Custome.css';
import ProductManagement from './Components/ProductManagement';

const App = () => (
  <FluentProvider theme={webLightTheme} className="min-vh-100">
    <div className="app-wrapper">
      <ProductManagement />
    </div>
  </FluentProvider>
);

export default App;