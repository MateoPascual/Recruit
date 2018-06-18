import * as server from "./server";
import React, { Fragment } from "react";
import { AsyncTypeahead } from "react-bootstrap-typeahead";
import { connect } from "react-redux";

class UserTypeAhead extends React.Component {
  state = {
    isLoading: false,
    SendingValue: "",
    Users: []
  };

  _handleSearch = query => {
    this.setState({ SendingValue: query }, () => this.sendToAjax());
  };

  sendToAjax = () => {
    const userTypeId = this.props.lookupData.UserType.filter(
      l => l.name == this.props.userType
    );
    this.setState({ isLoading: true });
    if (this.state.SendingValue.length) {
      const data = {};
      data.name = this.state.SendingValue;
      if (this.props.userType) {
        data.userTypeId = userTypeId[0].id;
      } //method to select the first mdn
      const myPromise = server.UserTypeAhead(data);
      myPromise.then(data => {
        if (data.data.items.length <= 50) {
          if (this.props.onChoices) {
            this.props.onChoices(data.data.items);
          }
          this.setState({ Users: data.data.items });
          this.setState({ isLoading: false });
        }
      });
    }
  };

  render() {
    return (
      <Fragment>
        <AsyncTypeahead
          labelKey="userName"
          minLength={3}
          isLoading={this.state.isLoading}
          onSearch={this._handleSearch}
          onChange={selected => {
            if (this.props.onChange) {
              this.props.onChange(selected);
            } else {
              this.props.onChange;
            }
          }}
          placeholder="Search for a User..."
          options={this.state.Users}
        />
      </Fragment>
    );
  }
}

function mapStateToProps(state) {
  return {
    lookupData: state.lookupData
  };
}

export default connect(mapStateToProps, null)(UserTypeAhead);

///////////////////////////////////////////////////////////////////////////////////
//
//                                 FOR USING MULTI SELECT
//
//
//     put in state
//          multiple: false,
//          allowNew: false,
//
//     use the checkbox component to use multi Selection
//          _renderCheckboxes()
//          {
//               const checkboxes =
//            [
//                   {label: 'Multi-Select', name: 'multiple'},
//                   {label: 'Allow custom selections', name: 'allowNew'},
//               ];
//              return checkboxes.map(({label, name}) =>
//              (
//                  <Control
//                   checked={this.state[name]}
//                    key={name}
//                    name={name}
//                    onChange={this._handleChange}
//                    type="checkbox">
//                    {label}
//                  </Control>
//              ));
//           }
//       _handleChange = (e) =>
//        {
//         const {checked, name} = e.target;
//         this.setState({[name]: checked});
//        }
//
///////////////////////////////////////////////////////////////////////////////////
