//
//  SearchBar.swift
//  Achiever
//
//  Created by Tom Brewer on 2/11/24.
//  Copyright © 2024 Brewer. All rights reserved.
//

import Foundation
import SwiftUI
// Create a new SearchBar view
struct SearchBar: View {
    @Binding var text: String

    var body: some View {
        TextField("Search...", text: $text)
    }
}
