import * as server from "./server";
import React, { Fragment } from "react";
import { AsyncTypeahead } from "react-bootstrap-typeahead";

class SchoolTypeAhead extends React.Component {
  state = {
    isLoading: false,
    SendingValue: "",
    schools: []
  };

  _handleSearch = query => {
    this.setState({ SendingValue: query }, () => this.sendToAjax());
  };

  sendToAjax = () => {
    this.setState({ isLoading: true });
    if (this.state.SendingValue.length) {
      const data = {};
      data.name = this.state.SendingValue;
      const myPromise = server.typeAhead(data);
      myPromise.then(data => {
        if (data.data.items.length <= 50) {
          this.setState({ schools: data.data.items });
          this.setState({ isLoading: false });
        }
      });
    }
  };

  render() {
    return (
      <Fragment>
        <AsyncTypeahead
          labelKey="schoolName"
          minLength={3}
          isLoading={this.state.isLoading}
          onSearch={this._handleSearch}
          placeholder="Search for a School..."
          options={this.state.schools}
        />
      </Fragment>
    );
  }
}

export default SchoolTypeAhead;
